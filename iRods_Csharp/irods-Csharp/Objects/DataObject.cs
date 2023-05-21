using System;
using Enums.Options;
using irods_Csharp;
using irods_Csharp.Objects;

namespace Objects.Objects;

/// <summary>
/// Class representing a Data Object on the server, can be used to read from and write to
/// </summary>
public class DataObject : ITaggable, IDisposable
{
    internal int Descriptor;

    private readonly IrodsSession _session;

    public string Path { get; }

    public string MetaType => "-d";

    /// <summary>
    /// DataObj constructor
    /// </summary>
    /// <param name="descriptor">FileDescriptor received from server</param>
    /// <param name="path">Path to data object</param>
    /// <param name="session">Session used to perform methods</param>
    internal DataObject(int descriptor, string path, IrodsSession session)
    {
        _session = session;
        Descriptor = descriptor;
        Path = path;
    }

    /// <summary>
    /// Send closing message to server indicating the user is done using the file
    /// </summary>
    public void Dispose()
    {
        Packet<OpenedDataObjInpPi> descRequest = new (ApiNumberData.DATA_OBJ_CLOSE_AN)
        {
            MsgBody = new OpenedDataObjInpPi(Descriptor, 0, 0, 0, 0, 0)
            {
                KeyValPairPi = new KeyValPairPi(0, null, null)
            }
        };

        _session.Connection.SendPacket(descRequest);

        _session.Connection.ReceivePacket();
    }

    /// <summary>
    /// Write content to end of file
    /// </summary>
    /// <param name="file">Data to write</param>
    public void Write(byte[] file)
    {
        Packet<OpenedDataObjInpPi> writeRequest = new (ApiNumberData.DATA_OBJ_WRITE_AN)
        {
            MsgBody = new OpenedDataObjInpPi(Descriptor, file.Length, 0, 0, 0, 0)
            {
                KeyValPairPi = new KeyValPairPi(0, null, null)
            },
            Binary = file
        };
        _session.Connection.SendPacket(writeRequest);

        _session.Connection.ReceivePacket();
    }

    /// <summary>
    /// Write content at beginning of file
    /// </summary>
    /// <param name="file">Data to write</param>
    public void Insert(byte[] file)
    {
        int current = Seek(0, SeekMode.Offset);
        byte[] content = Read();
        Seek(current, SeekMode.Start);
        Write(file);
        Write(content);
        Seek(-content.Length, SeekMode.End);
    }

    /// <summary>
    /// Get pointer to part of file
    /// </summary>
    /// <param name="offset">Offset from which to seek</param>
    /// <param name="seekMode">From where offset should be placed</param>
    /// <returns>Pointer to place in file</returns>
    public int Seek(int offset, SeekMode seekMode)
    {
        Packet<OpenedDataObjInpPi> readRequest = new (ApiNumberData.DATA_OBJ_LSEEK_AN)
        {
            MsgBody = new OpenedDataObjInpPi(Descriptor, 0, (int)seekMode, 0, offset, 0)
            {
                KeyValPairPi = new KeyValPairPi(0, null, null)
            }
        };
        _session.Connection.SendPacket(readRequest);

        Packet<FileLseekOutPi> readReply = _session.Connection.ReceivePacket<FileLseekOutPi>();

        return readReply.MsgBody.Offset;
    }

    /// <summary>
    /// Read data from data object
    /// </summary>
    /// <param name="length">Amount of bytes to read</param>
    /// <returns>Contents of data object</returns>
    public byte[] Read(int length = -1)
    {
        if (length == -1) length = Left();

        Packet<OpenedDataObjInpPi> readRequest = new (ApiNumberData.DATA_OBJ_READ_AN)
        {
            MsgBody = new OpenedDataObjInpPi(Descriptor, length, 0, 0, 0, 0)
            {
                KeyValPairPi = new KeyValPairPi(0, null, null)
            }
        };
        _session.Connection.SendPacket(readRequest);

        Packet readReply = _session.Connection.ReceivePacket();

        return readReply.Binary ?? Array.Empty<byte>();
    }

    /// <summary>
    /// Remove data object from server
    /// </summary>
    public void Remove()
    {
        _session.RemoveDataObject(Path);
    }

    /// <summary>
    /// Returns the amount of bytes left after the file pointer.
    /// </summary>
    /// <returns>The amount of bytes left.</returns>
    public int Left()
    {
        int current = Seek(0, SeekMode.Offset);
        int end = Seek(0, SeekMode.End);
        Seek(current, SeekMode.Start);
        return end - current;
    }

    /// <summary>
    /// Gets metadata attached to data object
    /// </summary>
    /// <param name="maxRows">Maximum amount of metadata rows to query</param>
    /// <returns>Metadata attached to data object</returns>
    public Metadata[] QueryMetadata(int maxRows = 500)
    {
        return _session.QueryMetadataPath(Path, "-d", maxRows);
    }

    /// <summary>
    /// Adds metadata to this data object
    /// </summary>
    /// <param name="name">Metadata name</param>
    /// <param name="value">Metadata value</param>
    /// <param name="units">Metadata units</param>
    public void AddMetadata(string name, string value, int units = -1)
    {
        _session.AddMetadata(this, name, value, units);
    }

    /// <summary>
    /// Removes metadata from this dataobject
    /// </summary>
    /// <param name="name">Metadata name</param>
    /// <param name="value">Metadata value</param>
    /// <param name="units">Metadata units</param>
    public void RemoveMetadata(string name, string value, int units = -1)
    {
        _session.RemoveMetadata(this, name, value, units);
    }
}