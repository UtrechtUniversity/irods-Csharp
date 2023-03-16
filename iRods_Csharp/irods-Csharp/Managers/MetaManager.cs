namespace irods_Csharp;

public class MetaManager
{
    private readonly IrodsSession _session;
    private readonly Path _home;

    /// <summary>
    /// Constructor for metadata manager.
    /// </summary>
    /// <param name="session">Session which contains account and connection</param>
    /// <param name="home">Path to home directory</param>
    public MetaManager(IrodsSession session, string home)
    {
        _session = session;
        _home = new Path(home);
    }

    /// <summary>
    /// Add metadata to taggable object.
    /// </summary>
    /// <param name="obj">Object to which metadata should be added</param>
    /// <param name="name">Metadata name</param>
    /// <param name="value">Metadata value</param>
    /// <param name="units">Metadata units, these are optional</param>
    public void AddMeta(ITaggable obj,  string name, string value, int units = -1)
    {
        Packet<ModAVUMetadataInp_PI> addMetaRequest = new (ApiNumberData.MOD_AVU_METADATA_AN)
        {
            MsgBody = new ModAVUMetadataInp_PI("add", obj.MetaType(), _home + obj.Path(), name, value, units)
        };

        _session.SendPacket(addMetaRequest);

        _session.ReceivePacket<None>();
    }

    /// <summary>
    /// Removes metadata from taggable object.
    /// </summary>
    /// <param name="obj">Object from which metadata should be removed</param>
    /// <param name="name">Metadata name</param>
    /// <param name="value">Metadata value</param>
    /// <param name="units">Metadata units, these are optional</param>
    // TODO test removal of meta tags with and without units
    public void RemoveMeta(ITaggable obj, string name, string value, int units = -1)
    {
        Packet<ModAVUMetadataInp_PI> removeMetaRequest = new (ApiNumberData.MOD_AVU_METADATA_AN)
        {
            MsgBody = new ModAVUMetadataInp_PI("rm", obj.MetaType(), _home + obj.Path(), name, value, units)
        };

        _session.SendPacket(removeMetaRequest);

        _session.ReceivePacket<None>();
    }
}