using Enums.Options;
using Objects.Objects;

namespace irods_Csharp.Objects;

/// <summary>
/// Class representing a Data Object on the server, can be opened to use.
/// </summary>
public class DataObjectReference
{
    private readonly IrodsSession _session;

    public DataObjectReference(IrodsSession session, int id, string path)
    {
        _session = session;
        Id = id;
        Path = path;
    }

    public int Id { get;  }
    public string Path { get; }

    /// <summary>
    /// Open the data object.
    /// </summary>
    /// <param name="mode">The file mode for the data objects.</param>
    /// <returns></returns>
    public DataObject Open(
        FileMode mode = FileMode.ReadWrite) =>
        _session.OpenDataObject(Path, mode);
    //TODO fix path
}