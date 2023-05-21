using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace irods_Csharp;

public class Packet
{
    [XmlElement("MsgHeader")]
    public MsgHeaderPi MsgHeader { get; set; }

    public byte[]? MsgBodyBytes { get; set; }
    
    [XmlIgnore]
    public byte[]? ErrorBytes { get; set; }

    [XmlElement("Error")]
    public RErrorPi? Error
    {
        get => ErrorBytes == null ? null : MessageSerializer.Deserialize<RErrorPi>(ErrorBytes);
        set => ErrorBytes = value == null ? null : MessageSerializer.Serialize(value);
    }

    [XmlElement("Binary")]
    public byte[]? Binary { get; set; }

    public Packet()
    {
        MsgHeader = null!;
    }

    public Packet(int intInfo = 0, string type = MessageType.API_REQ)
    {
        MsgHeader = new MsgHeaderPi(type, 0, 0, 0, intInfo);
    }

    public override string ToString()
    {
        XmlWriterSettings prettySettings = new() { OmitXmlDeclaration = true, Indent = true };
        XmlSerializerNamespaces emptyNameSpaces = new(new[] { XmlQualifiedName.Empty });

        XmlSerializer serializer = new(GetType());
        using StringWriter output = new();
        using XmlWriter writer = XmlWriter.Create(output, prettySettings);
        serializer.Serialize(writer, this, emptyNameSpaces);
        return output.ToString();
    }
}

public class Packet<T>
    where T : Message, new()
{
    [XmlElement("MsgHeader")]
    public MsgHeaderPi MsgHeader { get; set; }

    [XmlIgnore]
    public byte[]? MsgBodyBytes { get; set; }

    [XmlElement("MsgBody")]
    public T? MsgBody
    {
        get => MsgBodyBytes == null ? null : MessageSerializer.Deserialize<T>(MsgBodyBytes);
        set => MsgBodyBytes = value == null ? null : MessageSerializer.Serialize(value);
    }

    [XmlIgnore]
    public byte[]? ErrorBytes { get; set; }

    [XmlElement("Error")]
    public RErrorPi? Error
    {
        get => ErrorBytes == null ? null : MessageSerializer.Deserialize<RErrorPi>(ErrorBytes);
        set => ErrorBytes = value == null ? null : MessageSerializer.Serialize(value);
    }

    [XmlElement("Binary")]
    public byte[]? Binary { get; set; }

    public Packet()
    {
        MsgHeader = null!;
    }

    public Packet(int intInfo = 0, string type = MessageType.API_REQ)
    {
        MsgHeader = new MsgHeaderPi(type, 0, 0, 0, intInfo);
    }

    public override string ToString()
    {
        XmlWriterSettings prettySettings = new () { OmitXmlDeclaration = true, Indent = true };
        XmlSerializerNamespaces emptyNameSpaces = new (new[] { XmlQualifiedName.Empty });

        XmlSerializer serializer = new (GetType());
        using StringWriter output = new ();
        using XmlWriter writer = XmlWriter.Create(output, prettySettings);
        serializer.Serialize(writer, this, emptyNameSpaces);
        return output.ToString();
    }
}