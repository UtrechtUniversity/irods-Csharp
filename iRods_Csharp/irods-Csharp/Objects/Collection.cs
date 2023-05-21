using Objects.Objects;
using static irods_Csharp.PathUtil;
using FileMode = Enums.Options.FileMode;

// ReSharper disable once InconsistentNaming

namespace irods_Csharp.Objects;

/// <summary>
/// Class representing an irods collection
/// </summary>
public class Collection : ITaggable
{
    private readonly IrodsSession _session;

    public int Id { get; }
    public string Path { get; }

    /// <summary>
    /// Collection constructor
    /// </summary>
    /// <param name="path">Path to this collection</param>
    /// <param name="id">Collection id</param>
    /// <param name="session">Session used to perform methods</param>
    internal Collection(IrodsSession session, int id, string path)
    {
        _session = session;
        Id = id;
        Path = path;
    }

    /// <summary>
    /// Type needed for metadata operations
    /// </summary>
    /// <value>-c</value>
    public string MetaType => "-c";

    /// <summary>
    /// Rename this collection
    /// </summary>
    /// <param name="newName">New name for collection</param>
    public void Rename(string newName)
    {
        string newPath = PathCombine(First(Path), newName);
        _session.RenameCollection(Path, newPath);
    }

    #region DataObject

    /// <summary>
    /// Opens data object.
    /// </summary>
    /// <param name="path">Path to data object</param>
    /// <param name="fileMode">File mode used to open the object (Read, Write or ReadWrite)</param>
    /// <param name="truncate">Clear file when opening</param>
    /// <param name="create">Create file if it does not yet exist</param>
    /// <returns>DataObj object which can be used to read from or write to</returns>
    public DataObject OpenDataObj(string path, FileMode fileMode, bool truncate = false, bool create = false)
    {
        string newPath = PathCombine(Path, path);
        return _session.OpenDataObject(newPath, fileMode, truncate, create);
    }

    /// <summary>
    /// Renames data object.
    /// </summary>
    /// <param name="source">Original name of data object</param>
    /// <param name="target">New name of data object</param>
    public void RenameDataObj(string source, string target)
    {
        _session.RenameDataObject(PathCombine(Path, source), PathCombine(Path, target));
    }

    /// <summary>
    /// Write to data object
    /// </summary>
    /// <param name="path">Path to data object</param>
    /// <param name="file">Byte array which should be written</param>
    public void WriteDataObj(string path, byte[] file)
    {
        _session.WriteDataObject(PathCombine(Path, path), file);
    }

    /// <summary>
    /// Read file contents of data object
    /// </summary>
    /// <param name="path">Path to data object</param>
    /// <param name="length">How many bytes should be read</param>
    /// <returns>Content of data object in byte array form</returns>
    public byte[] ReadDataObj(string path, int length = -1)
    {
        return _session.ReadDataObject(PathCombine(Path, path), length);
    }

    /// <summary>
    /// Remove data object
    /// </summary>
    /// <param name="path">Path to parent collection of object</param>
    public void RemoveDataObj(string path)
    {
        _session.RemoveDataObject(PathCombine(Path, path));
    }

    #endregion

    #region Collection

    /// <summary>
    /// Looks for collection on server.
    /// </summary>
    /// <param name="path">Path to collection parent</param>
    /// <returns>Collection object</returns>
    public Collection OpenCollection(string path)
    {
        string newPath = PathCombine(Path, path);
        return _session.OpenCollection(newPath);
    }

    /// <summary>
    /// Renames collection within current collection.
    /// </summary>
    /// <param name="source">Original name of collection</param>
    /// <param name="target">new name of collection</param>
    public void RenameCollection(string source, string target)
    {
        _session.RenameCollection(PathCombine(Path, source), PathCombine(Path, target));
    }

    /// <summary>
    /// Creates collection.
    /// </summary>
    /// <param name="name">Name of new collection</param>
    public void CreateCollection(string name)
    {
        _session.CreateCollection(PathCombine(Path, name));
    }

    /// <summary>
    /// Removes collection.
    /// </summary>
    /// <param name="path">Path to collection parent</param>
    /// <param name="recursive">Delete items of collection if they exist</param>
    public void RemoveCollection(string path, bool recursive = true)
    {
        _session.RemoveCollection(PathCombine(Path, path), recursive);
    }

    #endregion

    #region Query

    /// <summary>
    /// Queries collections based on name.
    /// </summary>
    /// <param name="name">Name of collection which should be matched</param>
    /// <param name="strict">Whether or not the collection name should match exactly</param>
    /// <param name="maxRows">Maximum amount of rows to query</param>
    /// <returns>Array of matching collections</returns>
    public Collection[] QueryCollection(string name, bool strict = false, int maxRows = 500)
    {
        return _session.QueryCollectionPath(Path, name, strict, maxRows);
    }

    /// <summary>
    /// Queries collections based on metadata
    /// </summary>
    /// <param name="metaName">Name of meta triplet to search</param>
    /// <param name="metaValue">Value of meta triplet to search</param>
    /// <param name="metaUnits">Units of meta triplet to search</param>
    /// <param name="maxRows">Maximum amount of rows to query</param>
    /// <returns>Array of matching collections</returns>
    public Collection[] MQueryCollection(
        string metaName = "",
        string metaValue = "",
        int metaUnits = -1,
        int maxRows = 500
    )
    {
        return _session.MQueryCollection(Path, metaName, metaValue, metaUnits, maxRows);
    }

    /// <summary>
    /// Queries objects based on name
    /// </summary>
    /// <param name="name">Name of Data Object which should be matched</param>
    /// <param name="maxRows">Maximum amount of rows to query</param>
    /// <returns>Array of matching objects</returns>
    public DataObjectReference[] QueryDataObject(string name, int maxRows = 500)
    {
        return _session.QueryDataObjectPath(name, Path, maxRows);
    }

    /// <summary>
    /// Queries data objects based on metadata
    /// </summary>
    /// <param name="metaName">Name of meta triplet to search</param>
    /// <param name="metaValue">Value of meta triplet to search</param>
    /// <param name="metaUnits">Units of meta triplet to search</param>
    /// <param name="maxRows">Maximum amount of rows to query</param>
    /// <returns>Array of matching objects</returns>
    public DataObjectReference[] MQueryDataObject(
        string metaName = "",
        string metaValue = "",
        int metaUnits = -1,
        int maxRows = 500
    )
    {
        return _session.MQueryDataObject(Path, metaName, metaValue, metaUnits, maxRows);
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
        return _session.QueryMetadataPath(Path, MetaType, maxRows);
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