namespace irods_Csharp.Enums;

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