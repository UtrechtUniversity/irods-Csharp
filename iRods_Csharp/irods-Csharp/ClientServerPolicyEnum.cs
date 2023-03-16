namespace irods_Csharp;

public enum ClientServerPolicyRequest
{
    /// <summary>
    /// Require SSL
    /// </summary>
    RequireSSL,

    /// <summary>
    /// Require tcp without SSL.
    /// </summary>
    RefuseSSL
}


public enum ClientServerPolicyResult
{
    /// <summary>
    /// Use SSL
    /// </summary>
    useSSL,

    /// <summary>
    /// Use tcp without SSL.
    /// </summary>
    useTCP,

    /// <summary>
    /// Couldn't find common protocol.
    /// </summary>
    failure
}