using System;
using Objects;
using Objects.Objects;
using static irods_Csharp.Options;

// ReSharper disable once InconsistentNaming

namespace irods_Csharp.Objects;

/// <summary>
/// Class representing an irods collection
/// </summary>
public class Collection : ITaggable
{
    internal int Id;
    private readonly IrodsSession _session;
    internal Path _path;

    /// <summary>
    /// Collection constructor
    /// </summary>
    /// <param name="path">Path to this collection</param>
    /// <param name="id">Collection id</param>
    /// <param name="session">Session used to perform methods</param>
    internal Collection(Path path, int id, IrodsSession session)
    {
        Id = id;
        _path = path;
        _session = session;
    }

    /// <summary>
    /// Type needed for metadata operations
    /// </summary>
    /// <value>-c</value>
    public string MetaType => "-c";

    /// <summary>
    /// Path to this collection
    /// </summary>
    /// <value>Path to this collection in string format</value>
    public string Path => _path.ToString();

    /// <summary>
    /// Changes directory of this collection, while checking if the new collection exists
    /// </summary>
    /// <param name="path">Path to change to</param>
    public void ChangeDirectory(string path)
    {
        try
        {
            // Go up in directories for every ../ in the path
            while (path.StartsWith(".."))
            {
                path = path.Length > 3 ? path[3..] : "";
                int i = _path.ToString().LastIndexOf("/", StringComparison.Ordinal);
                _path = new Path(_path.ToString()[..i]);
                if (_path.ToString() != "")
                {
                    Collection[] collections = _session.QueryCollection("", _path.ToString()[1..], true);
                    Id = collections[0].Id;
                }
                else
                {
                    Id = _session.HomeCollection().Id;
                }
                if (path.Length == 0) break;
            }
            // Check if the collection exists, then change path
            if (path.Length > 0)
            {
                Collection[] collections = QueryCollection(path, true);
                if (collections.Length > 1) throw new Exception("Multiple ambiguous collections query");
                _path += new Path(path);
                Id = collections[0].Id;
            }
        }
        catch (Exception e) when (e.Message == "CAT_NO_ROWS_FOUND")
        {
            throw new Exception("Collection doesn't exist");
        }
    }

    /// <summary>
    /// Rename this collection
    /// </summary>
    /// <param name="newName">New name for collection</param>
    public void Rename(string newName)
    {
        int index = _path.ToString().LastIndexOf('/');
        Path path = new (_path.ToString()[..index]);
        _session.RenameCollection(_path, path+newName);
    }

    #region DataObject

    /// <summary>
    /// Opens data object.
    /// </summary>
    /// <param name="name">Path to data object</param>
    /// <param name="fileMode">File mode used to open the object (Read, Write or ReadWrite)</param>
    /// <param name="truncate">Clear file when opening</param>
    /// <param name="create">Create file if it does not yet exist</param>
    /// <returns>DataObj object which can be used to read from or write to</returns>
    public DataObject OpenDataObj(string name, FileMode fileMode, bool truncate = false, bool create = false)
    {
        return _session.OpenDataObject(_path + name, fileMode, truncate, create);
    }

    /// <summary>
    /// Renames data object.
    /// </summary>
    /// <param name="source">Original name of data object</param>
    /// <param name="target">New name of data object</param>
    public void RenameDataObj(string source, string target)
    {
        _session.RenameDataObject(_path + source, _path + target);
    }

    /// <summary>
    /// Create new data object
    /// </summary>
    /// <param name="path">Path where data object should be created, including name</param>
    public void CreateDataObj(string path)
    {
        _session.CreateDataObject(_path + path);
    }

    /// <summary>
    /// Write to data object
    /// </summary>
    /// <param name="path">Path to data object</param>
    /// <param name="file">Byte array which should be written</param>
    public void WriteDataObj(string path, byte[] file)
    {
        _session.WriteDataObject(_path + path, file);
    }

    /// <summary>
    /// Read file contents of data object
    /// </summary>
    /// <param name="path">Path to data object</param>
    /// <param name="length">How many bytes should be read</param>
    /// <returns>Content of data object in byte array form</returns>
    public byte[] ReadDataObj(string path, int length = -1)
    {
        return _session.ReadDataObject(_path + path, length);
    }

    /// <summary>
    /// Remove data object
    /// </summary>
    /// <param name="path">Path to parent collection of object</param>
    public void RemoveDataObj(string path)
    {
        _session.RemoveDataObject(_path + path);
    }

    #endregion

    #region Collection
    /// <summary>
    /// Looks for collection on server.
    /// </summary>
    /// <param name="name">Path to collection parent</param>
    /// <param name="id">Id of collection, will be queried if not supplied</param>
    /// <returns>Collection object</returns>
    public Collection OpenCollection(string name, int id = -1)
    {
        return _session.OpenCollection(_path + name, id);
    }

    /// <summary>
    /// Renames collection within current collection.
    /// </summary>
    /// <param name="source">Original name of collection</param>
    /// <param name="target">new name of collection</param>
    public void RenameCollection(string source, string target)
    {
        _session.RenameCollection(_path + source, _path + target);
    }

    /// <summary>
    /// Creates collection.
    /// </summary>
    /// <param name="name">Name of new collection</param>
    public void CreateCollection(string name)
    {
        _session.CreateCollection(_path + name);
    }

    /// <summary>
    /// Removes collection.
    /// </summary>
    /// <param name="path">Path to collection parent</param>
    /// <param name="recursive">Delete items of collection if they exist</param>
    public void RemoveCollection(string path, bool recursive = true)
    {
        _session.RemoveCollection(_path+path, recursive);
    }
    #endregion

    #region Query

    /// <summary>
    /// Perform general query with supplied conditions and select statements, casts results to supplied type.
    /// </summary>
    /// <param name="select">Array of table columns which should be queried</param>
    /// <param name="conditions">Array of conditions for query</param>
    /// <param name="type">Type to which query results should be cast</param>
    /// <param name="maxRows">Maximum amount of rows to query</param>
    /// <returns>Array of objects of the supplied type</returns>
    private object Query(Column[] select, Condition[] conditions, Type type, int maxRows = 500)
    {
        return _session.Query(_path, select, conditions, type, maxRows);
    }

    /// <summary>
    /// Queries collections based on name.
    /// </summary>
    /// <param name="name">Name of collection which should be matched</param>
    /// <param name="strict">Whether or not the collection name should match exactly</param>
    /// <param name="maxRows">Maximum amount of rows to query</param>
    /// <returns>Array of matching collections</returns>
    public Collection[] QueryCollection(string name, bool strict = false, int maxRows = 500)
    {
        return _session.QueryCollection(_path, name, strict, maxRows);
    }

    /// <summary>
    /// Queries collections based on metadata
    /// </summary>
    /// <param name="metaName">Name of meta triplet to search</param>
    /// <param name="metaValue">Value of meta triplet to search</param>
    /// <param name="metaUnits">Units of meta triplet to search</param>
    /// <param name="maxRows">Maximum amount of rows to query</param>
    /// <returns>Array of matching collections</returns>
    public Collection[] MQueryCollection(string metaName = "", string metaValue = "", int metaUnits = -1, int maxRows = 500)
    {
        return _session.MQueryCollection(_path, metaName,metaValue, metaUnits, maxRows);
    }

    /// <summary>
    /// Queries objects based on name
    /// </summary>
    /// <param name="name">Name of Data Object which should be matched</param>
    /// <param name="maxRows">Maximum amount of rows to query</param>
    /// <returns>Array of matching objects</returns>
    public DataObject[] QueryDataObject(string name, int maxRows = 500)
    {
        return _session.QueryDataObject(name, _path, Id, maxRows);
    }

    /// <summary>
    /// Queries data objects based on metadata
    /// </summary>
    /// <param name="metaName">Name of meta triplet to search</param>
    /// <param name="metaValue">Value of meta triplet to search</param>
    /// <param name="metaUnits">Units of meta triplet to search</param>
    /// <param name="maxRows">Maximum amount of rows to query</param>
    /// <returns>Array of matching objects</returns>
    public DataObject[] MQueryDataObject(string metaName = "", string metaValue = "", int metaUnits = -1, int maxRows = 500)
    {
        return _session.MQueryDataObject(_path, metaName, metaValue, metaUnits, Id, maxRows);
    }

    #endregion

    #region Meta

    /// <summary>
    /// Query metadata attached to this collection
    /// </summary>
    /// <param name="maxRows">Maximum amount of rows to query</param>
    /// <returns>Array of Meta structs that belong to this object</returns>
    public Metadata[] QueryMetadata(int maxRows = 500)
    {
        return _session.QueryMetadata(_path, MetaType, maxRows);
    }

    /// <summary>
    /// Add metadata to collection.
    /// </summary>
    /// <param name="name">Metadata name</param>
    /// <param name="value">Metadata value</param>
    /// <param name="units">Metadata units, these are optional</param>
    public void AddMetadata(string name, string value, int units = -1)
    {
        _session.AddMetadata(this, name, value, units);
    }

    /// <summary>
    /// Removes metadata from collection.
    /// </summary>
    /// <param name="name">Metadata name</param>
    /// <param name="value">Metadata value</param>
    /// <param name="units">Metadata units, these are optional</param>
    public void RemoveMetadata(string name, string value, int units = -1)
    {
        _session.RemoveMetadata(this, name, value, units);
    }
    #endregion
}