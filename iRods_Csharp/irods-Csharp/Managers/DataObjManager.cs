using System;

namespace irods_Csharp;

public class DataObjectManager
{
    internal IrodsSession Session;
    private readonly Path _home;

    /// <summary>
    /// Constructor for data object manager
    /// </summary>
    /// <param name="session">Session which contains account and connection</param>
    /// <param name="home">Path to home directory</param>
    public DataObjectManager(IrodsSession session, string home)
    {
        Session = session;
        _home = new Path(home);
    }

    /// <summary>
    /// Opens data object.
    /// </summary>
    /// <param name="path">Path to data object</param>
    /// <param name="fileMode">File mode used to open the object (Read, Write or ReadWrite)</param>
    /// <param name="truncate">Clear file when opening</param>
    /// <param name="create">Create file if it does not yet exist</param>
    /// <returns>DataObj object which can be used to read from or write to</returns>
    public DataObj Open(string path, Options.FileMode fileMode, bool truncate = false, bool create = false)
    {
        int flag = (int)fileMode;
        if (truncate) flag |= 512;
        const int cmode = 0644;

        Packet<DataObjInp_PI> descRequest;
        if (create) descRequest = new Packet<DataObjInp_PI>(ApiNumberData.DATA_OBJ_CREATE_AN)
        {
            MsgBody = new DataObjInp_PI(_home + path, cmode, flag, 0, -1, 0, 0)
            {
                KeyValPair_PI = new KeyValPair_PI(0, null, null)
            }
        };
        else descRequest = new Packet<DataObjInp_PI>(ApiNumberData.DATA_OBJ_OPEN_AN)
        {
            MsgBody = new DataObjInp_PI(_home + path, 0, flag, 0, -1, 0, 0)
            {
                KeyValPair_PI = new KeyValPair_PI(0, null, null)
            }
        };
        Session.SendPacket(descRequest);

        Packet<None> descReply = Session.ReceivePacket<None>();
        if(descReply.MsgHeader.intInfo == 0) throw new Exception("File does not exist.");
        return new DataObj(descReply.MsgHeader.intInfo, new Path(path), this);
    }

    /// <summary>
    /// Create new data object
    /// </summary>
    /// <param name="path">Path where data object should be created, including name</param>
    public void Create(string path)
    {
        using (Open(path, Options.FileMode.Read, true, true))
        {
        }
    }

    /// <summary>
    /// Write to data object
    /// </summary>
    /// <param name="path">Path to data object</param>
    /// <param name="file">Byte array which should be written</param>
    public void Write(string path,byte[] file)
    {
        using DataObj dataObj = Open(path, Options.FileMode.Write, true);
        dataObj.Write(file);
    }

    /// <summary>
    /// Read file contents of data object
    /// </summary>
    /// <param name="path">Path to data object</param>
    /// <param name="length">How many bytes should be read</param>
    /// <returns>Content of data object in byte array form</returns>
    public byte[] Read(string path, int length = -1)
    {
        using DataObj dataObj = Open(path, Options.FileMode.Read);
        return dataObj.Read(length);
    }

    /// <summary>
    /// Remove data object
    /// </summary>
    /// <param name="path">Path to parent collection of object</param>
    public void Remove(string path)
    {
        Packet<DataObjInp_PI> removeRequest = new (ApiNumberData.DATA_OBJ_UNLINK_AN)
        {
            MsgBody = new DataObjInp_PI(_home + path, 0, 0, 0, -1, 0, 0)
            {
                KeyValPair_PI = new KeyValPair_PI(0, null, null)
            }
        };
        Session.SendPacket(removeRequest);

        Session.ReceivePacket<None>();
    }
}