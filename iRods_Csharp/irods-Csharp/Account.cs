using System;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

// ReSharper disable InconsistentNaming

namespace irods_Csharp
{
    /// <summary>
    /// Class holding authentication information
    /// </summary>
    internal class Account
    {
        public string irods_host;
        public int irods_port;
        public string irods_home { get; }
        private string irods_user_name;
        private string irods_zone_name;
        public int TTL { get; }
        public string irods_authentication_scheme;

        /// <summary>
        /// Account constructor
        /// </summary>
        /// <param name="irods_host">Irods host server</param>
        /// <param name="irods_port">Port on which to access server</param>
        /// <param name="irods_home">Path of home collection</param>
        /// <param name="irods_user_name">Username of user</param>
        /// <param name="irods_zone_name">Zone name of server</param>
        /// <param name="irods_authentication_scheme">Authentication scheme to be used</param>
        /// <param name="ttl">The hour the password secret will stay valid</param>
        public Account(string irods_host, int irods_port, string irods_home, string irods_user_name, string irods_zone_name, string irods_authentication_scheme, int ttl)
        {
            this.irods_host = irods_host;
            this.irods_port = irods_port;
            this.irods_home = irods_home;
            this.irods_user_name = irods_user_name;
            this.irods_zone_name = irods_zone_name;
            this.irods_authentication_scheme = irods_authentication_scheme;
            TTL = ttl;
        }

        /// <summary>
        /// Creates a context string for the pam authentication using the account details
        /// </summary>
        /// <param name="password">The users password.</param>
        /// <returns>The context string</returns>
        public string PamContext(string password)
        {
            return string.Join(";", $"a_user={irods_user_name}", $"a_pw={password}", $"a_ttl={TTL}");
        }

        /// <summary>
        /// Creates startup pack used to establish connection with server
        /// </summary>
        /// <returns>StartupPack_PI Irods Message</returns>
        public StartupPack_PI MakeStartupPack()
        {
            return new StartupPack_PI(Options.iRODSProt_t.XML_PROT, 0, 0, irods_user_name, irods_zone_name, irods_user_name, irods_zone_name, "rods4.2.6", "d", "");
        }

        /// <summary>
        /// Generates authentication response to secure connection with server
        /// </summary>
        /// <param name="password">User password</param>
        /// <param name="challenge">TODO jelle </param>
        /// <returns></returns>
        public authResponseInp_PI GenerateAuthResponse(string password, string challenge)
        {
            password = password.PadRight(50, '\0');
            challenge = Encoding.UTF8.GetString(Convert.FromBase64String(challenge));
            string response;

            using (MD5 md5 = new MD5CryptoServiceProvider())
            {
                byte[] bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(challenge + password));
                response = Convert.ToBase64String(bytes);
            }

            return new authResponseInp_PI(response, irods_user_name);
        }

        /// <summary>
        /// Concatenates fields in string format
        /// </summary>
        /// <returns>String containing all fields</returns>
        public override string ToString()
        {
            string result = string.Empty;
            result += "{\n";
            foreach (FieldInfo field in GetType().GetFields()) result += $"\"{field.Name}\":\"{field.GetValue(this)}\",\n";
            result += "}";
            return result;
        }
    }
}
