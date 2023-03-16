using System;
using static irods_Csharp.Options.SeekMode;

namespace irods_Csharp;

/// <summary>
/// Class representing a Data Object on the server, can be used to read from and write to
/// </summary>
public class DataObj : ITaggable, IDisposable
{
    internal int Descriptor;

    private readonly DataObjectManager _manager;
    private readonly Path _path;

    public string Path() => _path;

    public string MetaType() => "-d";

    /// <summary>
    /// DataObj constructor
    /// </summary>
    /// <param name="descriptor">FileDescriptor received from server</param>
    /// <param name="path">Path to data object</param>
    /// <param name="manager">DataObjManager used to perform methods</param>
    internal DataObj(int descriptor, Path path, DataObjectManager manager)
    {
        Descriptor = descriptor;
        _path = path;
        _manager = manager;
    }

    /// <summary>
    /// Send closing message to server indicating the user is done using the file
    /// </summary>
    public void Dispose()
    {
        Packet<OpenedDataObjInp_PI> descRequest = new (ApiNumberData.DATA_OBJ_CLOSE_AN)
        {
            MsgBody = new OpenedDataObjInp_PI(Descriptor, 0, 0, 0, 0, 0)
            {
                KeyValPair_PI = new KeyValPair_PI(0, null, null)
            }
        };
        _manager.Session.SendPacket(descRequest);

        _manager.Session.ReceivePacket<None>();
    }

    /// <summary>
    /// Write content to end of file
    /// </summary>
    /// <param name="file">Data to write</param>
    public void Write(byte[] file)
    {
        Packet<OpenedDataObjInp_PI> writeRequest = new (ApiNumberData.DATA_OBJ_WRITE_AN)
        {
            MsgBody = new OpenedDataObjInp_PI(Descriptor, file.Length, 0, 0, 0, 0)
            {
                KeyValPair_PI = new KeyValPair_PI(0, null, null)
            },
            Binary = file
        };
        _manager.Session.SendPacket(writeRequest);

        _manager.Session.ReceivePacket<None>();
    }

    /// <summary>
    /// Write content at beginning of file
    /// </summary>
    /// <param name="file">Data to write</param>
    public void Insert(byte[] file)
    {
        int current = Seek(0, Offset);
        byte[] content = Read();
        Seek(current, Start);
        Write(file);
        Write(content);
        Seek(-content.Length, End);
    }

    /// <summary>
    /// Get pointer to part of file
    /// </summary>
    /// <param name="offset">Offset from which to seek</param>
    /// <param name="seekMode">From where offset should be placed</param>
    /// <returns>Pointer to place in file</returns>
    public int Seek(int offset, Options.SeekMode seekMode)
    {
        Packet<OpenedDataObjInp_PI> readRequest = new (ApiNumberData.DATA_OBJ_LSEEK_AN)
        {
            MsgBody = new OpenedDataObjInp_PI(Descriptor, 0, (int)seekMode, 0, offset, 0)
            {
                KeyValPair_PI = new KeyValPair_PI(0, null, null)
            }
        };
        _manager.Session.SendPacket(readRequest);

        Packet<fileLseekOut_PI> readReply = _manager.Session.ReceivePacket<fileLseekOut_PI>();

        return readReply.MsgBody.offset;
    }

    /// <summary>
    /// Read data from data object
    /// </summary>
    /// <param name="length">Amount of bytes to read</param>
    /// <returns>Contents of data object</returns>
    public byte[] Read(int length = -1)
    {
        if (length == -1) length = Left();

        Packet<OpenedDataObjInp_PI> readRequest = new (ApiNumberData.DATA_OBJ_READ_AN)
        {
            MsgBody = new OpenedDataObjInp_PI(Descriptor, length, 0, 0, 0, 0)
            {
                KeyValPair_PI = new KeyValPair_PI(0, null, null)
            }
        };
        _manager.Session.SendPacket(readRequest);

        Packet<None> readReply = _manager.Session.ReceivePacket<None>();

        return readReply.Binary ?? new byte[0];
    }

    /// <summary>
    /// Remove data object from server
    /// </summary>
    public void Remove()
    {
        _manager.Remove(_path);
    }

    /// <summary>
    /// Returns the amount of bytes left after the file pointer.
    /// </summary>
    /// <returns>The amount of bytes left.</returns>
    public int Left()
    {
        int current = Seek(0, Offset);
        int end = Seek(0, End);
        Seek(current, Start);
        return end - current;
    }

    /// <summary>
    /// Gets metadata attached to data object
    /// </summary>
    /// <param name="maxRows">Maximum amount of metadata rows to query</param>
    /// <returns>Metadata attached to data object</returns>
    public Meta[] Meta(int maxRows = 500)
    {
        return _manager.Session.Queries.QueryMeta(_path, "-d", maxRows);
    }

    /// <summary>
    /// Adds metadata to this data object
    /// </summary>
    /// <param name="name">Metadata name</param>
    /// <param name="value">Metadata value</param>
    /// <param name="units">Metadata units</param>
    public void AddMeta(string name, string value, int units = -1)
    {
        _manager.Session.Meta.AddMeta(this, name, value, units);
    }

    /// <summary>
    /// Removes metadata from this dataobject
    /// </summary>
    /// <param name="name">Metadata name</param>
    /// <param name="value">Metadata value</param>
    /// <param name="units">Metadata units</param>
    public void RemoveMeta(string name, string value, int units = -1)
    {
        _manager.Session.Meta.RemoveMeta(this, name, value, units);
    }
}