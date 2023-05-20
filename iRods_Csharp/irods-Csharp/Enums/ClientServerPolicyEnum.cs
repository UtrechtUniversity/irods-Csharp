// ReSharper disable InconsistentNaming
namespace irods_Csharp.Enums;

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