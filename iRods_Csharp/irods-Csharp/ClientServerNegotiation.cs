namespace irods_Csharp;

public class ClientServerNegotiation
{
    public ClientServerNegotiation(ClientServerPolicyRequest clientServerPolicy = ClientServerPolicyRequest.RequireSSL)
    {
        ClientServerPolicy = clientServerPolicy;
    }

    public ClientServerPolicyRequest ClientServerPolicy { get; set; }
}