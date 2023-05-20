using System;
using System.Collections.Generic;
using System.Linq;
using irods_Csharp.Enums;
using irods_Csharp.Objects;
using Objects.Objects;

namespace irods_Csharp;

/// <summary>
/// Class holding a Connection, Path and Account used to operate on the server.
/// </summary>
public class IrodsSession : IDisposable
{
    internal readonly Connection Connection;
    private readonly Account _account;
    private readonly Path _home;

    /// <summary>
    /// IrodsSession constructor.
    /// </summary>
    /// <param name="host">The host to connect to.</param>
    /// <param name="port">The port of the host.</param>
    /// <param name="home">The default directory.</param>
    /// <param name="user">The users name/email.</param>
    /// <param name="zone">The zone.</param>
    /// <param name="scheme">The authentication scheme.</param>
    /// <param name="ttl">The hour the password secret will stay valid</param>
    /// <param name="requestServerNegotiation">Optional request server negotiation.</param>
    public IrodsSession(
        string host,
        int port,
        string home,
        string user,
        string zone,
        AuthenticationScheme scheme,
        int ttl,
        ClientServerNegotiation? requestServerNegotiation
    )
    {
        _account = new Account(host, port, home, user, zone, scheme, ttl);
        _home = new Path(_account.Home);
        Connection = new Connection(_account, requestServerNegotiation);
    }

    /// <summary>
    /// Connect to the server and run the authentication scheme implementation.
    /// </summary>
    /// <param name="password">The user password.</param>
    /// <returns>The authentication scheme secret.</returns>
    public string Setup(string password)
    {
        Connection.Connect();

        string secret = _account.AuthenticationScheme switch
        {
            AuthenticationScheme.Pam => Connection.Pam(password),
            AuthenticationScheme.Native => Connection.Native(password),
            _ => throw new Exception("Authentication method not implemented.")
        };

        Connection.Dispose();

        return secret;
    }

    /// <summary>
    /// Connects to the server and authenticates using the authentication scheme secret.
    /// </summary>
    /// <param name="secret">The authentication scheme secret.</param>
    public void Start(string secret)
    {
        Connection.Connect();
        Connection.AuthenticationRequest(secret);
    }

    /// <summary>
    /// Disconnects from the server.
    /// </summary>
    public void Dispose()
    {
        Connection.Dispose();
    }

    /// <summary>
    /// Renames either collection or data object
    /// </summary>
    /// <param name="source">Original name</param>
    /// <param name="target">New name</param>
    /// <param name="isCollection">Whether the object is a collection or a data object</param>
    private void RenameCollectionOrDataObject(string source, string target, bool isCollection)
    {
        int type = isCollection ? 12 : 11;
        Path home = new (_account.Home);

        Packet<DataObjCopyInpPi> renameRequest = new (ApiNumberData.DATA_OBJ_RENAME_AN)
        {
            MsgBody = new DataObjCopyInpPi
            {
                Src = new DataObjInpPi(home + source, 0, 0, 0, 0, 0, type)
                {
                    KeyValPairPi = new KeyValPairPi(0, null, null)
                },
                Dest = new DataObjInpPi(home + target, 0, 0, 0, 0, 0, type)
                {
                    KeyValPairPi = new KeyValPairPi(0, null, null)
                },
            }
        };
        Connection.SendPacket(renameRequest);

        Connection.ReceivePacket<None>();
    }

    /// <summary>
    /// Home collection of the session
    /// </summary>
    /// <returns>Home collection of the session</returns>
    public Collection HomeCollection()
    {
        Collection[] colls = QueryCollection(
            QueryModels.Collection(),
            new[] { new Condition(QueryModels.COLL_NAME, "like", _account.Home) }
        );
        return colls[0];
    }

    #region DataObjects

    /// <summary>
    /// Renames data object.
    /// </summary>
    /// <param name="source">Original name of collection</param>
    /// <param name="target">New name of collection</param>
    public void RenameDataObject(string source, string target)
    {
        RenameCollectionOrDataObject(source, target, false);
    }

    /// <summary>
    /// Opens data object.
    /// </summary>
    /// <param name="path">Path to data object</param>
    /// <param name="fileMode">File mode used to open the object (Read, Write or ReadWrite)</param>
    /// <param name="truncate">Clear file when opening</param>
    /// <param name="create">Create file if it does not yet exist</param>
    /// <returns>DataObj object which can be used to read from or write to</returns>
    public DataObject OpenDataObject(string path, Options.FileMode fileMode, bool truncate = false, bool create = false)
    {
        int flag = (int)fileMode;
        if (truncate) flag |= 512;
        const int cmode = 0644;

        Packet<DataObjInpPi> descRequest;
        if (create)
            descRequest = new Packet<DataObjInpPi>(ApiNumberData.DATA_OBJ_CREATE_AN)
            {
                MsgBody = new DataObjInpPi(_home + path, cmode, flag, 0, -1, 0, 0)
                {
                    KeyValPairPi = new KeyValPairPi(0, null, null)
                }
            };
        else
            descRequest = new Packet<DataObjInpPi>(ApiNumberData.DATA_OBJ_OPEN_AN)
            {
                MsgBody = new DataObjInpPi(_home + path, 0, flag, 0, -1, 0, 0)
                {
                    KeyValPairPi = new KeyValPairPi(0, null, null)
                }
            };
        Connection.SendPacket(descRequest);

        Packet<None> descReply = Connection.ReceivePacket<None>();
        if (descReply.MsgHeader.IntInfo == 0) throw new Exception("File does not exist.");
        return new DataObject(descReply.MsgHeader.IntInfo, new Path(path), this);
    }

    /// <summary>
    /// Create new data object
    /// </summary>
    /// <param name="path">Path where data object should be created, including name</param>
    public void CreateDataObject(string path)
    {
        using (OpenDataObject(path, Options.FileMode.Read, true, true))
        {
        }
    }

    /// <summary>
    /// Write to data object
    /// </summary>
    /// <param name="path">Path to data object</param>
    /// <param name="file">Byte array which should be written</param>
    public void WriteDataObject(string path, byte[] file)
    {
        using DataObject dataObj = OpenDataObject(path, Options.FileMode.Write, true);
        dataObj.Write(file);
    }

    /// <summary>
    /// Read file contents of data object
    /// </summary>
    /// <param name="path">Path to data object</param>
    /// <param name="length">How many bytes should be read</param>
    /// <returns>Content of data object in byte array form</returns>
    public byte[] ReadDataObject(string path, int length = -1)
    {
        using DataObject dataObj = OpenDataObject(path, Options.FileMode.Read);
        return dataObj.Read(length);
    }

    /// <summary>
    /// Remove data object
    /// </summary>
    /// <param name="path">Path to parent collection of object</param>
    public void RemoveDataObject(string path)
    {
        Packet<DataObjInpPi> removeRequest = new (ApiNumberData.DATA_OBJ_UNLINK_AN)
        {
            MsgBody = new DataObjInpPi(_home + path, 0, 0, 0, -1, 0, 0)
            {
                KeyValPairPi = new KeyValPairPi(0, null, null)
            }
        };
        Connection.SendPacket(removeRequest);

        Connection.ReceivePacket<None>();
    }

    #endregion

    #region Metadata

    /// <summary>
    /// Add metadata to taggable object.
    /// </summary>
    /// <param name="obj">Object to which metadata should be added</param>
    /// <param name="name">Metadata name</param>
    /// <param name="value">Metadata value</param>
    /// <param name="units">Metadata units, these are optional</param>
    public void AddMetadata(ITaggable obj, string name, string value, int units = -1)
    {
        Packet<ModAvuMetadataInpPi> addMetaRequest = new (ApiNumberData.MOD_AVU_METADATA_AN)
        {
            MsgBody = new ModAvuMetadataInpPi("add", obj.MetaType, _home + obj.Path, name, value, units)
        };

        Connection.SendPacket(addMetaRequest);

        Connection.ReceivePacket<None>();
    }

    /// <summary>
    /// Removes metadata from taggable object.
    /// </summary>
    /// <param name="obj">Object from which metadata should be removed</param>
    /// <param name="name">Metadata name</param>
    /// <param name="value">Metadata value</param>
    /// <param name="units">Metadata units, these are optional</param>
    // TODO test removal of meta tags with and without units
    public void RemoveMetadata(ITaggable obj, string name, string value, int units = -1)
    {
        Packet<ModAvuMetadataInpPi> removeMetaRequest = new (ApiNumberData.MOD_AVU_METADATA_AN)
        {
            MsgBody = new ModAvuMetadataInpPi("rm", obj.MetaType, _home + obj.Path, name, value, units)
        };

        Connection.SendPacket(removeMetaRequest);

        Connection.ReceivePacket<None>();
    }

    #endregion

    #region Query

    /// <summary>
    /// Perform general query with supplied conditions and select statements, casts results to supplied type.
    /// </summary>
    /// <param name="select">Array of table columns which should be queried</param>
    /// <param name="conditions">Array of conditions for query</param>
    /// <param name="maxRows">Maximum amount of rows to query</param>
    /// <returns>Array of objects of the supplied type</returns>
    private GenQueryOutPi Query(Column[] select, Condition[] conditions, int maxRows = 500)
    {
        InxIvalPairPi selects = new (
            select.Length,
            select.Select(x => x.Id).ToArray(),
            Enumerable.Repeat(1, select.Length).ToArray()
        );
        InxValPairPi cConditions = new (
            conditions.Length,
            conditions.Select(x => x.Column.Id).ToArray(),
            conditions.Select(x => x.ToString()).ToArray()
        );

        //TODO Implement keyword conditions
        KeyValPairPi kConditions = new (0, null, null);

        Packet<GenQueryInpPi> query = new (ApiNumberData.GEN_QUERY_AN)
        {
            MsgBody = new GenQueryInpPi(maxRows, 0, 0, 0, kConditions, selects, cConditions)
        };

        Connection.SendPacket(query);

        Packet<GenQueryOutPi> queryResult = Connection.ReceivePacket<GenQueryOutPi>();

        return queryResult.MsgBody!;
    }

    /// <summary>
    /// Perform general query with supplied conditions and select statements, casts results to supplied type.
    /// </summary>
    /// <param name="select">Array of table columns which should be queried</param>
    /// <param name="conditions">Array of conditions for query</param>
    /// <param name="maxRows">Maximum amount of rows to query</param>
    /// <returns>Array of collections.</returns>
    internal Collection[] QueryCollection(Column[] select, Condition[] conditions, int maxRows = 500)
    {
        GenQueryOutPi queryResult = Query(select, conditions, maxRows);

        const int collectionNameColumn = 1, collectionIdColumn = 0;
        return Enumerable.Range(0, queryResult.RowCnt)
            .Select(
                i => new Collection(
                    new Path(queryResult.SqlResultPi[collectionNameColumn].Value[i].Replace(_home.ToString(), "")),
                    int.Parse(queryResult.SqlResultPi[collectionIdColumn].Value[i]),
                    this
                )
            )
            .ToArray();
    }

    /// <summary>
    /// Perform general query with supplied conditions and select statements, casts results to supplied type.
    /// </summary>
    /// <param name="path">The path of the data object.</param>
    /// <param name="select">Array of table columns which should be queried</param>
    /// <param name="conditions">Array of conditions for query</param>
    /// <param name="mode">The file mode for the data objects.</param>
    /// <param name="maxRows">Maximum amount of rows to query</param>
    /// <returns>Array of objects.</returns>
    public DataObject[] QueryDataObject(
        string path,
        Column[] select,
        Condition[] conditions,
        Options.FileMode mode = Options.FileMode.ReadWrite,
        int maxRows = 500
    )
    {
        GenQueryOutPi queryResult = Query(select, conditions.ToArray(), maxRows);

        const int objNameColumn = 2;
        return Enumerable.Range(0, queryResult.RowCnt)
            .Select(
                i =>
                {

                    // TODO remove path and use from result.
                    string newName = queryResult.SqlResultPi[objNameColumn].Value[i];
                    DataObject dataObj = OpenDataObject(path + newName, mode);
                    return dataObj;
                }
            )
            .ToArray();
    }


    /// <summary>
    /// Perform general query with supplied conditions and select statements, casts results to supplied type.
    /// </summary>
    /// <param name="select">Array of table columns which should be queried</param>
    /// <param name="conditions">Array of conditions for query</param>
    /// <param name="maxRows">Maximum amount of rows to query</param>
    /// <returns>Array of metadata.</returns>
    public Metadata[] QueryMetadata(
        Column[] select,
        Condition[] conditions,
        int maxRows = 500
    )
    {
        GenQueryOutPi queryResult = Query(select, conditions, maxRows);

        const int metaNameColumn = 0, metaKeywordColumn = 1, metaUnitsColumn = 2;
        return Enumerable.Range(0, queryResult.RowCnt)
            .Select(
                i =>
                {
                    string unitValue = queryResult.SqlResultPi[metaUnitsColumn].Value[i];
                    int? units = unitValue == "" ? null : int.Parse(unitValue);
                    return new Metadata(
                        queryResult.SqlResultPi[metaNameColumn].Value[i],
                        queryResult.SqlResultPi[metaKeywordColumn].Value[i],
                        units
                    );
                }
            )
            .ToArray();
    }

    /// <summary>
    /// Queries collections based on name.
    /// </summary>
    /// <param name="path">Path of collection where should be searched</param>
    /// <param name="name">Name of collection which should be matched</param>
    /// <param name="strict">Whether or not the collection name should match exactly</param>
    /// <param name="maxRows">Maximum amount of rows to query</param>
    /// <returns>Array of matching collections</returns>
    public Collection[] QueryCollectionPath(string path, string name, bool strict = false, int maxRows = 500)
    {
        Condition[] conditions = strict
            ? new[] { new Condition(QueryModels.COLL_NAME, "=", _home + path + "/" + name) }
            : new[]
            {
                new Condition(QueryModels.COLL_NAME, "like", _home + path + "%"),
                new Condition(QueryModels.COLL_NAME, "like", "%" + name + "%")
            };

        return QueryCollection(QueryModels.Collection(), conditions, maxRows);
    }

    /// <summary>
    /// Queries objects based on name
    /// </summary>
    /// <param name="name">Name of Data Object which should be matched</param>
    /// <param name="path">Path of collection where should be searched</param>
    /// <param name="collectionId">Id of parent collection, will be queried if left unspecified</param>
    /// <param name="maxRows">Maximum amount of rows to query</param>
    /// <param name="mode">The file mode for the data objects.</param>
    /// <returns>Array of matching objects</returns>
    public DataObject[] QueryDataObjectPath(
        string name,
        string path,
        int collectionId = -1,
        int maxRows = 500,
        Options.FileMode mode = Options.FileMode.ReadWrite
    )
    {
        collectionId = CollectionCheck(collectionId, path);

        List<Condition> conditions = new ()
        {
            new Condition(QueryModels.DATA_NAME, "like", "%" + name + "%"),
            new Condition(QueryModels.D_COLL_ID, "=", collectionId.ToString())
        };

        return QueryDataObject(path, QueryModels.DataObject(), conditions.ToArray(), mode, maxRows);
    }

    /// <summary>
    /// Queries collections based on metadata
    /// </summary>
    /// <param name="path">Path of collection where should be searched</param>
    /// <param name="metaName">Name of meta triplet to search</param>
    /// <param name="metaValue">Value of meta triplet to search</param>
    /// <param name="metaUnits">Units of meta triplet to search</param>
    /// <param name="maxRows">Maximum amount of rows to query</param>
    /// <returns>Array of matching collections</returns>
    public Collection[] MQueryCollection(
        string path,
        string metaName = "",
        string metaValue = "",
        int metaUnits = -1,
        int maxRows = 500
    )
    {
        List<Condition> conditions = new ()
        {
            new Condition(QueryModels.COLL_NAME, "like", _home + "%"),
            new Condition(QueryModels.COLL_NAME, "like", "%" + path + "/%")
        };

        if (metaName != "") conditions.Add(new Condition(QueryModels.COL_META_COLL_ATTR_NAME, "=", metaName));
        if (metaValue != "") conditions.Add(new Condition(QueryModels.COL_META_COLL_ATTR_VALUE, "=", metaValue));
        if (metaUnits >= 0)
            conditions.Add(new Condition(QueryModels.COL_META_COLL_ATTR_UNITS, "=", metaUnits.ToString()));

        return QueryCollection(QueryModels.Collection(), conditions.ToArray(), maxRows);
    }

    /// <summary>
    /// Queries data objects based on metadata
    /// </summary>
    /// <param name="path">Path of collection where should be searched</param>
    /// <param name="metaName">Name of meta triplet to search</param>
    /// <param name="metaValue">Value of meta triplet to search</param>
    /// <param name="metaUnits">Units of meta triplet to search</param>
    /// <param name="collectionId">Id of parent collection, will be queried if left unspecified</param>
    /// <param name="maxRows">Maximum amount of rows to query</param>
    /// <returns>Array of matching objects</returns>
    public DataObject[] MQueryDataObject(
        string path,
        string metaName = "",
        string metaValue = "",
        int metaUnits = -1,
        int collectionId = -1,
        int maxRows = 500
    )
    {
        collectionId = CollectionCheck(collectionId, path);

        List<Condition> conditions = new ();
        if (path != "") conditions.Add(new Condition(QueryModels.D_COLL_ID, "=", collectionId.ToString()));

        if (metaName != "") conditions.Add(new Condition(QueryModels.COL_META_DATA_ATTR_NAME, "=", metaName));
        if (metaValue != "") conditions.Add(new Condition(QueryModels.COL_META_DATA_ATTR_VALUE, "=", metaValue));
        if (metaUnits >= 0)
            conditions.Add(new Condition(QueryModels.COL_META_DATA_ATTR_UNITS, "=", metaUnits.ToString()));

        return QueryDataObject(path, QueryModels.DataObject(), conditions.ToArray(), Options.FileMode.ReadWrite, maxRows);
    }

    /// <summary>
    /// Finds id corresponding to path, if this isn't given.
    /// </summary>
    /// <param name="collectionId">Id of collection, if it negative then the id still needs to be found</param>
    /// <param name="path">Path to collection</param>
    /// <returns>Collection id</returns>
    private int CollectionCheck(int collectionId, string path)
    {
        if (collectionId < 0)
        {
            Collection[] coll = QueryCollection(
                QueryModels.Collection(),
                new[] { new Condition(QueryModels.COLL_NAME, "like", _home + path) }
            );
            collectionId = coll[0].Id;
        }

        return collectionId;
    }

    /// <summary>
    /// Returns meta tags attached to object.
    /// </summary>
    /// <param name="path">Path to parent collection of object</param>
    /// <param name="type">Type of object</param>
    /// <param name="maxRows">Maximum amount of rows to query</param>
    /// <returns>Array of metadata</returns>
    public Metadata[] QueryMetadataPath(string path, string type, int maxRows = 500)
    {
        Condition[] conditions = Array.Empty<Condition>();
        Column[] select = Array.Empty<Column>();
        switch (type)
        {
            case "-c":
                conditions = new[] { new Condition(QueryModels.COLL_NAME, "=", _home + path) };
                select = QueryModels.CollectionMeta();
                break;
            case "-d":
                Collection[] coll = QueryCollection(
                    QueryModels.Collection(),
                    new[] { new Condition(QueryModels.COLL_NAME, "=", _home + Path.First(path)) }
                );
                int collectionId = coll[0].Id;
                conditions = new[]
                {
                    new Condition(QueryModels.DATA_NAME, "=", Path.Last(path)),
                    new Condition(QueryModels.D_COLL_ID, "=", collectionId.ToString())
                };
                select = QueryModels.DataObjMeta();
                break;
        }

        return QueryMetadata(select, conditions, maxRows);
    }

    #endregion

    #region Collection

    /// <summary>
    /// Renames collection.
    /// </summary>
    /// <param name="source">Original name of collection</param>
    /// <param name="target">New name of collection</param>
    public void RenameCollection(string source, string target)
    {
        RenameCollectionOrDataObject(source, target, true);
    }

    /// <summary>
    /// Looks for collection on server.
    /// </summary>
    /// <param name="path">Path to collection parent</param>
    /// <param name="id">Id of collection, will be queried if not supplied</param>
    /// <returns>Collection object</returns>
    public Collection OpenCollection(string path, int id = -1)
    {
        if (id == -1)
        {
            id = QueryCollectionPath(Path.First(path), Path.Last(path), true)[0].Id;
        }

        return new Collection(new Path(path), id, this);
    }

    /// <summary>
    /// Creates collection.
    /// </summary>
    /// <param name="path">Path where collection should be created, including name</param>
    public void CreateCollection(string path)
    {
        KeyValPairPi mkdirRequestMsgPair = new (0, Array.Empty<string>(), Array.Empty<string>());
        Packet<CollInpNewPi> mkdirRequest = new (ApiNumberData.COLL_CREATE_AN)
        {
            MsgBody = new CollInpNewPi(_home + path, 0, 0, mkdirRequestMsgPair)
        };
        Connection.SendPacket(mkdirRequest);

        Connection.ReceivePacket<None>();
    }

    /// <summary>
    /// Removes collection.
    /// </summary>
    /// <param name="path">Path to collection parent</param>
    /// <param name="recursive">Delete items of collection if they exist</param>
    public void RemoveCollection(string path, bool recursive = true)
    {
        Packet<CollInpNewPi> rmdirRequest = new (ApiNumberData.RM_COLL_AN)
        {
            MsgBody = new CollInpNewPi(_home + path, 0, 0)
            {
                KeyValPairPi = recursive
                    ? new KeyValPairPi(1, new[] { "recursiveOpr" }, new[] { "" })
                    : new KeyValPairPi(0, Array.Empty<string>(), Array.Empty<string>())
            }
        };
        Connection.SendPacket(rmdirRequest);

        Connection.ReceivePacket<CollOprStatPi>();
    }

    #endregion
}