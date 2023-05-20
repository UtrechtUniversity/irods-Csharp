using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace irods_Csharp;

/// <summary>
/// Class for message (de)serialization
/// </summary>
internal static class MessageSerializer
{
    private static readonly XmlWriterSettings Settings = new()
    {
        OmitXmlDeclaration = true,
        NamespaceHandling = NamespaceHandling.Default
    };

    private static readonly XmlWriterSettings PrettySettings = new()
    {
        OmitXmlDeclaration = true,
        Indent = true
    };

    private static readonly XmlSerializerNamespaces EmptyNameSpaces = new (new[] { XmlQualifiedName.Empty });

    /// <summary>
    /// Turns the object into a XML string.
    /// </summary>
    /// <typeparam name="T">The type of the IRodsMessage</typeparam>
    /// <param name="message">The message itself.</param>
    /// <returns>The string in XML format</returns>
    public static string Stringify<T>(T message) where T : Message
    {
        XmlSerializer serializer = new(typeof(T));
        using StringWriter output = new();
        using XmlWriter writer = XmlWriter.Create(output, PrettySettings);
        serializer.Serialize(writer, message, EmptyNameSpaces);
        return output.ToString();
    }

    /// <summary>
    /// Parse the given string into a IRodsMessage.
    /// </summary>
    /// <typeparam name="T">The type of the deserialized IRodsMessage.</typeparam>
    /// <param name="s">The string to parse.</param>
    /// <returns>The deserialized object.</returns>
    internal static T Parse<T>(string s) where T : Message, new()
    {
        XmlSerializer deserializer = new(typeof(T));
        using StringReader output = new(s);
        using XmlTextReader reader = new(output);
        return (T)deserializer.Deserialize(reader)!;
    }

    /// <summary>
    /// Turns the object into bytes.
    /// </summary>
    /// <typeparam name="T">The type of the IRodsMessage</typeparam>
    /// <param name="message">The message itself.</param>
    /// <returns>The bytes</returns>
    public static byte[] Serialize<T>(T message) where T : Message
    {
        XmlSerializer serializer = new(typeof(T));
        using StringWriter output = new();
        using XmlWriter writer = XmlWriter.Create(output, Settings);
        serializer.Serialize(writer, message, EmptyNameSpaces);
        return Encoding.UTF8.GetBytes(output.ToString());
    }

    /// <summary>
    /// Deserializes the given byte[] into a IRodsMessage.
    /// </summary>
    /// <typeparam name="T">The type of the deserialized IRodsMessage.</typeparam>
    /// <param name="bytes">The byte[] to deserialize.</param>
    /// <returns>The deserialized object.</returns>
    internal static T Deserialize<T>(byte[] bytes) where T : Message, new()
    {
        string content = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
        XmlSerializer deserializer = new(typeof(T));
        using StringReader output = new(content);
        using XmlTextReader reader = new(output);
        return (T)deserializer.Deserialize(reader)!;
    }
}