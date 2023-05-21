using System;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Linq;
using static irods_Csharp.Utility;
using System.Security.Cryptography;
using irods_Csharp.Enums;

namespace irods_Csharp;

/// <summary>
/// Class holding a networkstream or sslstream to the server and the methods needed to use it.
/// </summary>
internal class Connection : IDisposable
{
    private readonly Account _account;
    private Stream _stream;

    /// <summary>
    /// Connection constructor.
    /// </summary>
    /// <param name="account">Used for connecting and verification.</param>
    /// <param name="serverStream">Data stream</param>
    private Connection(Account account, Stream serverStream)
    {
        _account = account;
        _stream = serverStream;
    }

    /// <summary>
    /// Creates a connection to the server and sends a RODS_CONNECT message for identification.
    /// </summary>
    public static Connection CreateConnection(Account account, ClientServerNegotiation? requestServerNegotiation)
    {
        Socket serverSocket = new (SocketType.Stream, ProtocolType.Tcp);
        serverSocket.Connect(account.Host, account.Port);
        NetworkStream serverStream = new (serverSocket)
        {
            ReadTimeout = 100000
        };
        Connection connection = new (account, serverStream);

        if (requestServerNegotiation == null)
        {
            Packet<StartupPackPi> connectionRequest = new (type: MessageType.CONNECT)
            {
                MsgBody = account.MakeStartupPack()
            };
            connection.SendPacket(connectionRequest);

            connection.ReceivePacket<VersionPi>();
        }
        else
        {
            ClientServerPolicyRequest clientPolicy = requestServerNegotiation.ClientServerPolicy;

            Packet<StartupPackPi> connectionRequest = new (type: MessageType.CONNECT)
            {
                MsgBody = account.MakeStartupPack("request_server_negotiation")
            };
            connection.SendPacket(connectionRequest);

            ClientServerPolicyRequest serverPolicy = connection.ReceivePacket<CsNegPi>().MsgBody!.ServerPolicy;

            ClientServerPolicyResult agreedPolicy = (clientPolicy, serverPolicy) switch
            {
                (ClientServerPolicyRequest.RequireSSL, ClientServerPolicyRequest.RequireSSL)
                    or (ClientServerPolicyRequest.DontCare, ClientServerPolicyRequest.RequireSSL)
                    or (ClientServerPolicyRequest.RequireSSL, ClientServerPolicyRequest.DontCare) =>
                    ClientServerPolicyResult.UseSSL,
                (ClientServerPolicyRequest.RefuseSSL, ClientServerPolicyRequest.RefuseSSL)
                    or (ClientServerPolicyRequest.DontCare, ClientServerPolicyRequest.RefuseSSL)
                    or (ClientServerPolicyRequest.RefuseSSL, ClientServerPolicyRequest.DontCare) =>
                    ClientServerPolicyResult.UseTCP,
                // Default to ssl if both parties do not care
                (ClientServerPolicyRequest.DontCare, ClientServerPolicyRequest.DontCare) => ClientServerPolicyResult
                    .UseSSL,
                _ => ClientServerPolicyResult.Failure
            };

            Packet<CsNegPi> negotationRequest = new (type: MessageType.CLIENT_SERVER_NEGOTIATION)
            {
                MsgBody = new CsNegPi(agreedPolicy)
            };
            connection.SendPacket(negotationRequest);

            connection.ReceivePacket<VersionPi>();

            switch (agreedPolicy)
            {
                case ClientServerPolicyResult.Failure:
                    serverStream.Dispose();
                    throw new Exception(
                        $"Failed to negotiate policy, client wants: {clientPolicy} while server wants: {serverPolicy}"
                    );
                case ClientServerPolicyResult.UseSSL:
                    connection.SecureWithSecret(requestServerNegotiation);
                    break;
                case ClientServerPolicyResult.UseTCP:
                    // Continue as normal.
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        return connection;
    }

    /// <summary>
    /// Sends a RODS_DISCONNECT message to the server and disposes the serverStream.
    /// </summary>
    public void Dispose()
    {
        Packet disconnectRequest = new (type: MessageType.DISCONNECT);
        SendPacket(disconnectRequest);

        _stream.Dispose();
    }

    /// <summary>
    /// Asks the server to start SSL and creates and replaces the _serverStream NetworkStream with an SslStream.
    /// </summary>
    public void SecureWithSecret(ClientServerNegotiation negotiation)
    {
        SslStream secureServerStream = new (_stream, false);
        secureServerStream.AuthenticateAsClient(_account.Host);

        _stream = secureServerStream;

        Packet encryptionRequest = new ()
        {
            MsgHeader = new MsgHeaderPi(
                negotiation.Algorithm,
                negotiation.KeySize,
                negotiation.SaltSize,
                negotiation.HashRounds,
                0
            )
        };
        SendPacket(encryptionRequest);

        Packet keyRequest = new (type: "SHARED_SECRET")
        {
            MsgBodyBytes = RandomNumberGenerator.GetBytes(negotiation.KeySize)
        };
        SendPacket(keyRequest);
    }

    /// <summary>
    /// Asks the server to start SSL and creates and replaces the _serverStream NetworkStream with an SslStream.
    /// </summary>
    public void Secure()
    {
        Packet<SSLStartInpPi> sslRequest = new (ApiNumberData.SSL_START_AN)
        {
            MsgBody = new SSLStartInpPi("")
        };
        SendPacket(sslRequest);

        ReceivePacket();

        SslStream secureServerStream = new (_stream, false);
        secureServerStream.AuthenticateAsClient(_account.Host);

        _stream = secureServerStream;
    }

    /// <summary>
    /// Sends an authentication request to the server and uses the challenge from the response to authenticate.
    /// </summary>
    /// <param name="password">The account / PAM password.</param>
    public void AuthenticationRequest(string password)
    {
        Packet authRequest = new (ApiNumberData.AUTH_REQUEST_AN);
        SendPacket(authRequest);

        Packet<AuthRequestOutPi> authRequestReply = ReceivePacket<AuthRequestOutPi>();

        Packet<AuthResponseInpPi> authResponse = new (ApiNumberData.AUTH_RESPONSE_AN)
        {
            MsgBody = _account.GenerateAuthResponse(password, authRequestReply.MsgBody!.Challenge)
        };
        SendPacket(authResponse);

        ReceivePacket();
    }

    /// <summary>
    /// Sends the user password over SSL to the server to receive a new PAM password.
    /// </summary>
    /// <param name="password">The user password.</param>
    /// <returns>The PAM password.</returns>
    public string Pam(string password)
    {
        if (_stream is not SslStream) Secure();

        Packet<AuthPlugReqInpPi> msgHeaderStructClient = new (ApiNumberData.AUTH_PLUG_REQ_AN)
        {
            MsgBody = new AuthPlugReqInpPi("PAM", _account.PamContext(password))
        };
        SendPacket(msgHeaderStructClient);
        msgHeaderStructClient.MsgBody.Context = "REDACTED";

        Packet<AuthPlugReqOutPi> msgHeaderStructServer = ReceivePacket<AuthPlugReqOutPi>();

        return msgHeaderStructServer.MsgBody!.Result;
    }

    /// <summary>
    /// Authentication native.
    /// </summary>
    /// <param name="password">The user password.</param>
    /// <returns>The Native Password</returns>
    public string Native(string password)
    {
        if (_stream is not SslStream) Secure();

        return password;
    }

    /// <summary>
    /// Sends a packet without type to the server.
    /// </summary>
    /// <param name="packet">The packet to send to the server.</param>
    public void SendPacket(Packet packet)
    {
        WriteLog(ConsoleColor.DarkGray, packet);

        if (packet.MsgBodyBytes != null) packet.MsgHeader.MsgLen = packet.MsgBodyBytes.Length;
        if (packet.ErrorBytes != null) packet.MsgHeader.ErrorLen = packet.ErrorBytes.Length;
        if (packet.Binary != null) packet.MsgHeader.BsLen = packet.Binary.Length;

        byte[] msgHeaderClientBytes = MessageSerializer.Serialize(packet.MsgHeader);

        // Make sure the bytes are send out in the correct order.
        byte[] headerBytes = BitConverter.GetBytes(msgHeaderClientBytes.Length);
        if (BitConverter.IsLittleEndian) headerBytes = headerBytes.Reverse().ToArray();

        SendBytes(headerBytes);
        SendBytes(msgHeaderClientBytes);

        if (packet.MsgBodyBytes != null) SendBytes(packet.MsgBodyBytes);
        if (packet.ErrorBytes != null) SendBytes(packet.ErrorBytes);
        if (packet.Binary != null) SendBytes(packet.Binary);
    }

    /// <summary>
    /// Sends a packet of type T to the server.
    /// </summary>
    /// <typeparam name="T">The type of an IRodsMessage subclass.</typeparam>
    /// <param name="packet">The packet to send to the server.</param>
    public void SendPacket<T>(Packet<T> packet)
        where T : Message, new()
    {
        WriteLog(ConsoleColor.DarkGray, packet);
         
        if (packet.MsgBodyBytes != null) packet.MsgHeader.MsgLen = packet.MsgBodyBytes.Length;
        if (packet.ErrorBytes != null) packet.MsgHeader.ErrorLen = packet.ErrorBytes.Length;
        if (packet.Binary != null) packet.MsgHeader.BsLen = packet.Binary.Length;

        byte[] msgHeaderClientBytes = MessageSerializer.Serialize(packet.MsgHeader);

        // Make sure the bytes are send out in the correct order.
        byte[] headerBytes = BitConverter.GetBytes(msgHeaderClientBytes.Length);
        if (BitConverter.IsLittleEndian) headerBytes = headerBytes.Reverse().ToArray();

        SendBytes(headerBytes);
        SendBytes(msgHeaderClientBytes);

        if (packet.MsgBodyBytes != null) SendBytes(packet.MsgBodyBytes);
        if (packet.ErrorBytes != null) SendBytes(packet.ErrorBytes);
        if (packet.Binary != null) SendBytes(packet.Binary);
    }

    /// <summary>
    /// Receives a packet without type from the server.
    /// </summary>
    /// <returns>The packet received from the server.</returns>
    public Packet ReceivePacket()  
    {
        Packet packet = new() { MsgHeader = ReceiveMessage<MsgHeaderPi>() };
        if (packet.MsgHeader.MsgLen > 0) packet.MsgBodyBytes = ReceiveBytes(packet.MsgHeader.MsgLen);
        if (packet.MsgHeader.ErrorLen > 0) packet.ErrorBytes = ReceiveBytes(packet.MsgHeader.ErrorLen);
        if (packet.MsgHeader.BsLen > 0) packet.Binary = ReceiveBytes(packet.MsgHeader.BsLen);
        WriteLog(ConsoleColor.Gray, packet);
        if (packet.MsgHeader.IntInfo >= 0) return packet;

        Exception e =
            new(Table.ApiErrorData[packet.MsgHeader.IntInfo])
            {
                Data = { ["error"] = packet.Error }
            };
        throw e;
    }
    /// <summary>
    /// Receives a packet of type T from the server.
    /// </summary>
    /// <typeparam name="T">The type of an IRodsMessage subclass.</typeparam>
    /// <returns>The packet received from the server.</returns>
    public Packet<T> ReceivePacket<T>() where T : Message, new()
    {
        Packet<T> packet = new() { MsgHeader = ReceiveMessage<MsgHeaderPi>() };
        if (packet.MsgHeader.MsgLen > 0) packet.MsgBodyBytes = ReceiveBytes(packet.MsgHeader.MsgLen);
        if (packet.MsgHeader.ErrorLen > 0) packet.ErrorBytes = ReceiveBytes(packet.MsgHeader.ErrorLen);
        if (packet.MsgHeader.BsLen > 0) packet.Binary = ReceiveBytes(packet.MsgHeader.BsLen);
        WriteLog(ConsoleColor.Gray, packet);
        if (packet.MsgHeader.IntInfo >= 0) return packet;

        Exception e =
            new (Table.ApiErrorData[packet.MsgHeader.IntInfo])
            {
                Data = { ["error"] = packet.Error, ["body"] = packet.MsgBody }
            };
        throw e;
    }

    /// <summary>
    /// Receives an IRodsMessage from the server.
    /// </summary>
    /// <typeparam name="T">The type of an IRodsMessage subclass.</typeparam>
    /// <returns>The IRodsMessage received from the server.</returns>
    private T ReceiveMessage<T>() where T : Message, new() => MessageSerializer.Deserialize<T>(ReceiveBytes(BitConverter.ToInt32(ReceiveBytes(4).Reverse().ToArray(), 0)));

    /// <summary>
    /// Receives an IRodsMessage with the specified byte length from the server.
    /// </summary>
    /// <typeparam name="T">The type of an IRodsMessage subclass.</typeparam>
    /// <param name="length">The amount of bytes to receive from the server.</param>
    /// <returns>The IRodsMessage received from the server.</returns>
    private T ReceiveMessage<T>(int length) where T : Message, new() => MessageSerializer.Deserialize<T>(ReceiveBytes(length));

    /// <summary>
    /// Sends the specified bytes to the server with an optional 4 byte length header.
    /// </summary>
    /// <param name="bytes">The bytes to send to the server.</param>
    private void SendBytes(byte[] bytes)
    {
        if (bytes.Length == 0) return;
        _stream.Write(bytes, 0, bytes.Length);
    }

    /// <summary>
    /// Receive the specified amount of bytes from the server.
    /// </summary>
    /// <param name="length">The amount of bytes to receive from the server.</param>
    /// <returns>The bytes received from the server.</returns>
    private byte[] ReceiveBytes(int length)
    {
        int size = 0;
        byte[] output = new byte[length];
        while (size < length)
        {
            size += _stream.Read(output, size, length - size);
        }
        return output;
    }
}