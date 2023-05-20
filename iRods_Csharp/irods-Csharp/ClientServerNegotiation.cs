using irods_Csharp.Enums;

namespace irods_Csharp;

public class ClientServerNegotiation
{
    public ClientServerNegotiation(
        ClientServerPolicyRequest clientServerPolicy,
        string algorithm,
        int keySize,
        int saltSize,
        int hashRounds
    )
    {
        ClientServerPolicy = clientServerPolicy;
        Algorithm = algorithm;
        KeySize = keySize;
        SaltSize = saltSize;
        HashRounds = hashRounds;
    }

    public ClientServerPolicyRequest ClientServerPolicy { get; set; }
    public string Algorithm { get; set; }
    public int KeySize { get; set; }
    public int SaltSize { get; set; }
    public int HashRounds { get; set; }
}