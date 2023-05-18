﻿using System;

namespace irods_Csharp;

/// <summary>
/// Class holding a Connection, Path and Account used to operate on the server.
/// </summary>
public class IrodsSession : IDisposable
{
    private readonly Connection _connection;
    private readonly Account _account;

    public readonly CollectionManager Collections;
    public readonly DataObjectManager DataObjects;
    public readonly QueryManager Queries;
    public readonly MetaManager Meta;

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
    public IrodsSession(string host, int port, string home, string user, string zone, string scheme, int ttl, ClientServerNegotiation? requestServerNegotiation)
    {
        AuthenticationScheme aScheme = new AuthenticationScheme();

        switch (scheme.ToLower().Trim())
        {

            case "pam":
                aScheme = AuthenticationScheme.Pam;

                break;

            case "native":
                aScheme = AuthenticationScheme.native;
                break;

            default:
                aScheme = AuthenticationScheme.Pam;
                break;
        }


        _account = new Account(host, port, home, user, zone, aScheme, ttl);
        _connection = new Connection(_account, requestServerNegotiation);

        DataObjects = new DataObjectManager(this, _account.Home);
        Collections = new CollectionManager(this, _account.Home);
        Queries = new QueryManager(this, _account.Home);
        Meta = new MetaManager(this, _account.Home);
    }

    /// <summary>
    /// Connect to the server and run the authentication scheme implementation.
    /// </summary>
    /// <param name="password">The user password.</param>
    /// <returns>The authentication scheme secret.</returns>
    public string Setup(string password)
    {
        _connection.Connect();

        string secret = _account.AuthenticationScheme switch
        {
            AuthenticationScheme.Pam => _connection.Pam(password),
            AuthenticationScheme.native => _connection.Native(password),
            _ => throw new Exception("Authentication method not implemented.")
        };

        _connection.Dispose();

        return secret;
    }

    /// <summary>
    /// Connects to the server and authenticates using the authentication scheme secret.
    /// </summary>
    /// <param name="secret">The authentication scheme secret.</param>
    public void Start(string secret)
    {
        _connection.Connect();
        _connection.AuthenticationRequest(secret);
    }

    /// <summary>
    /// Disconnects from the server.
    /// </summary>
    public void Dispose()
    {
        _connection.Dispose();
    }

    /// <summary>
    /// Sends a packet of type T to the server.
    /// </summary>
    /// <typeparam name="T">The type of an IRodsMessage subclass.</typeparam>
    /// <param name="packet">The packet to send to the server.</param>
    internal void SendPacket<T>(Packet<T> packet) where T : Message, new()
    {
        _connection.SendPacket(packet);
    }

    /// <summary>
    /// Receives a packet of type T from the server.
    /// </summary>
    /// <typeparam name="T">The type of an IRodsMessage subclass.</typeparam>
    /// <returns>The packet received from the server.</returns>
    internal Packet<T> ReceivePacket<T>() where T : Message, new()
    {
        return _connection.ReceivePacket<T>();
    }

    /// <summary>
    /// Renames either collection or data object
    /// </summary>
    /// <param name="source">Original name</param>
    /// <param name="target">New name</param>
    /// <param name="isCollection">Whether the object is a collection or a data object</param>
    public void Rename(string source, string target, bool isCollection)
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
        SendPacket(renameRequest);

        ReceivePacket<None>();
    }

    /// <summary>
    /// Home collection of the session
    /// </summary>
    /// <returns>Home collection of the session</returns>
    public Collection HomeCollection()
    {
        Collection[] colls = (Collection[])Queries.Query("", QueryModels.Collection(), new[] { new Condition(QueryModels.COLL_NAME, "like", _account.Home) }, typeof(Collection));
        return colls[0];
    }
}