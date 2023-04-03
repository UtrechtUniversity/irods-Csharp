using System;
using System.Security.Cryptography;
using System.Text;

// ReSharper disable InconsistentNaming

namespace irods_Csharp;

/// <summary>
/// Possible authentication schemes.
/// </summary>
public enum AuthenticationScheme
{
    /// <summary>
    /// Pluggable Authentication Modules (PAM) authentication
    /// </summary>
    Pam
}

/// <summary>
/// Class holding authentication information
/// </summary>
internal class Account
{
    public string Host { get; }
    public int Port { get; }
    public string Home { get; }
    private readonly string _userName;
    private readonly string _zoneName;
    public AuthenticationScheme AuthenticationScheme { get; }
    public int TTL { get; }

    /// <summary>
    /// Account constructor
    /// </summary>
    /// <param name="host">Irods host server</param>
    /// <param name="port">Port on which to access server</param>
    /// <param name="home">Path of home collection</param>
    /// <param name="user_name">Username of user</param>
    /// <param name="zone_name">Zone name of server</param>
    /// <param name="authentication_scheme">Authentication scheme to be used</param>
    /// <param name="ttl">The hour the password secret will stay valid</param>
    public Account(string host, int port, string home, string user_name, string zone_name, AuthenticationScheme authentication_scheme, int ttl)
    {
        Host = host;
        Port = port;
        Home = home;
        _userName = user_name;
        _zoneName = zone_name;
        AuthenticationScheme = authentication_scheme;
        TTL = ttl;
    }

    /// <summary>
    /// Creates a context string for the pam authentication using the account details
    /// </summary>
    /// <param name="password">The users password.</param>
    /// <returns>The context string</returns>
    public string PamContext(string password) => $"a_user={_userName};a_pw={password};a_ttl={TTL}";

    /// <summary>
    /// Creates startup pack used to establish connection with server
    /// Thomas Overbergh updated this to use rods 4.3.0.
    /// </summary>
    /// <returns>StartupPack_PI Irods Message</returns>
    public StartupPackPi MakeStartupPack(string option = "") => new(Options.iRODSProt_t.XML_PROT, 0, 0, _userName, _zoneName, _userName, _zoneName, "rods4.3.0", "d", option);
    //public StartupPackPi MakeStartupPack(string option = "") => new (Options.iRODSProt_t.XML_PROT, 0, 0, _userName, _zoneName, _userName, _zoneName, "rods4.2.6", "d", option);

    /// <summary>
    /// Generates authentication response to secure connection with server
    /// </summary>
    /// <param name="password">User password</param>
    /// <param name="challenge">Auth challenge</param>
    /// <returns></returns>
    public AuthResponseInpPi GenerateAuthResponse(string password, string challenge)
    {
        password = password.PadRight(50, '\0');
        challenge = Encoding.UTF8.GetString(Convert.FromBase64String(challenge));

        byte[] bytes = MD5.HashData(Encoding.UTF8.GetBytes(challenge + password));
        string response = Convert.ToBase64String(bytes);

        return new AuthResponseInpPi(response, _userName);
    }
}