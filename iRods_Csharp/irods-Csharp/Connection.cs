using System;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Linq;
using static irods_Csharp.Utility;

namespace irods_Csharp;

/// <summary>
/// Class holding a networkstream or sslstream to the server and the methods needed to use it.
/// </summary>
internal class Connection : IDisposable
{
    private readonly Account _account;
    private readonly ClientServerNegotiation? _requestServerNegotiation;
    private Stream _serverStream;

    /// <summary>
    /// Connection constructor.
    /// </summary>
    /// <param name="account">Used for connecting and verification.</param>
    /// <param name="requestServerNegotiation"></param>
    public Connection(Account account, ClientServerNegotiation? requestServerNegotiation)
    {
        _account = account;
        _requestServerNegotiation = requestServerNegotiation;
        Connect();
    }

    /// <summary>
    /// Creates a connection to the server and sends a RODS_CONNECT message for identification.
    /// </summary>
    public void Connect()
    {
        Socket serverSocket = new (SocketType.Stream, ProtocolType.Tcp);
        serverSocket.Connect(_account.Host, _account.Port);
        _serverStream = new NetworkStream(serverSocket)
        {
            ReadTimeout = 5000
        };

        if (_requestServerNegotiation == null)
        {
            Packet<StartupPackPi> connectionRequest = new (type: MessageType.CONNECT)
            {
                MsgBody = _account.MakeStartupPack()
            };
            SendPacket(connectionRequest);
        }
        else
        {
            ClientServerPolicyRequest clientPolicy = _requestServerNegotiation.ClientServerPolicy;

            Packet<StartupPackPi> connectionRequest = new (type: MessageType.CONNECT)
            {
                MsgBody = _account.MakeStartupPack("request_server_negotiation")
            };
            SendPacket(connectionRequest);

            ClientServerPolicyRequest serverPolicy = ReceivePacket<CsNegPi>().MsgBody!.ServerPolicy;

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
            SendPacket(negotationRequest);

            if (agreedPolicy is ClientServerPolicyResult.Failure)
            {
                _serverStream.Dispose();
                throw new Exception(
                    $"Failed to negotiate policy, client wants: {clientPolicy} while server wants: {serverPolicy}"
                );
            }
        }

        ReceivePacket<VersionPi>();
    }

    /// <summary>
    /// Sends a RODS_DISCONNECT message to the server and disposes the serverStream.
    /// </summary>
    public void Dispose()
    {
        Packet<None> disconnectRequest = new (type: MessageType.DISCONNECT);
        SendPacket(disconnectRequest);

        _serverStream.Dispose();
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

        ReceivePacket<None>();

        SslStream secureServerStream = new (_serverStream, false);
        secureServerStream.AuthenticateAsClient(_account.Host);

        _serverStream = secureServerStream;
    }

    /// <summary>
    /// Sends an authentication request to the server and uses the challenge from the response to authenticate.
    /// </summary>
    /// <param name="password">The account / PAM password.</param>
    public void AuthenticationRequest(string password)
    {
        Packet<None> authRequest = new (ApiNumberData.AUTH_REQUEST_AN);
        SendPacket(authRequest);

        Packet<AuthRequestOutPi> authRequestReply = ReceivePacket<AuthRequestOutPi>();

        Packet<AuthResponseInpPi> authResponse = new (ApiNumberData.AUTH_RESPONSE_AN)
        {
            MsgBody = _account.GenerateAuthResponse(password, authRequestReply.MsgBody.Challenge)
        };
        SendPacket(authResponse);

        ReceivePacket<None>();
    }

    /// <summary>
    /// Sends the user password over SSL to the server to receive a new PAM password.
    /// </summary>
    /// <param name="password">The user password.</param>
    /// <returns>The PAM password.</returns>
    public string Pam(string password)
    {
        Secure();

        Packet<AuthPlugReqInpPi> msgHeaderStructClient = new (ApiNumberData.AUTH_PLUG_REQ_AN)
        {
            MsgBody = new AuthPlugReqInpPi("PAM", _account.PamContext(password))
        };
        SendPacket(msgHeaderStructClient);
        msgHeaderStructClient.MsgBody.Context = "REDACTED";

        Packet<AuthPlugReqOutPi> msgHeaderStructServer = ReceivePacket<AuthPlugReqOutPi>();

        return msgHeaderStructServer.MsgBody.Result;
    }

    /// <summary>
    /// Sends a packet of type T to the server.
    /// </summary>
    /// <typeparam name="T">The type of an IRodsMessage subclass.</typeparam>
    /// <param name="packet">The packet to send to the server.</param>
    public void SendPacket<T>(Packet<T> packet) where T : Message
    {
        WriteLog(ConsoleColor.DarkGray, packet);

        byte[]? msgBytes = null;
        byte[]? errorBytes = null;

        if (packet.MsgBody != null)
        {
            msgBytes = MessageSerializer.Serialize(packet.MsgBody);
            packet.MsgHeader.MsgLen = msgBytes.Length;
        }

        if (packet.Error != null)
        {
            errorBytes = MessageSerializer.Serialize(packet.Error);
            packet.MsgHeader.ErrorLen = errorBytes.Length;
        }

        if (packet.Binary != null)
        {
            packet.MsgHeader.BsLen = packet.Binary.Length;
        }

        byte[] msgHeaderClientBytes = MessageSerializer.Serialize(packet.MsgHeader);
        SendBytes(msgHeaderClientBytes, true);

        if (msgBytes != null) SendBytes(msgBytes, false);
        if (errorBytes != null) SendBytes(errorBytes, false);
        if (packet.Binary != null) SendBytes(packet.Binary, false);
    }

    /// <summary>
    /// Receives a packet of type T from the server.
    /// </summary>
    /// <typeparam name="T">The type of an IRodsMessage subclass.</typeparam>
    /// <returns>The packet received from the server.</returns>
    public Packet<T> ReceivePacket<T>() where T : Message, new()
    {
        Packet<T> packet = new() { MsgHeader = ReceiveMessage<MsgHeaderPi>() };
        if (packet.MsgHeader.MsgLen > 0) packet.MsgBody = ReceiveMessage<T>(packet.MsgHeader.MsgLen);
        if (packet.MsgHeader.ErrorLen > 0) packet.Error = ReceiveMessage<RErrorPi>(packet.MsgHeader.ErrorLen);
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
    /// <param name="header">Whether to send a 4 byte header or not.</param>
    private void SendBytes(byte[] bytes, bool header)
    {
        if (header) _serverStream.Write(BitConverter.GetBytes(bytes.Length).Reverse().ToArray(), 0, 4);
        _serverStream.Write(bytes, 0, bytes.Length);
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
            size += _serverStream.Read(output, size, length - size);
        }
        return output;
    }
}