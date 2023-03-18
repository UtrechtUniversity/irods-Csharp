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
    RefuseSSL,

    /// <summary>
    /// Either option is fine.
    /// </summary>
    DontCare
}


public enum ClientServerPolicyResult
{
    /// <summary>
    /// Use SSL
    /// </summary>
    UseSSL,

    /// <summary>
    /// Use tcp without SSL.
    /// </summary>
    UseTCP,

    /// <summary>
    /// Couldn't find common protocol.
    /// </summary>
    Failure
}