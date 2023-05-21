using System;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml.Serialization;
using Enums.Options;
using irods_Csharp.Enums;

// ReSharper disable EmptyConstructor
// ReSharper disable NotAccessedField.Local
// ReSharper disable UnusedMember.Global
[assembly: InternalsVisibleTo("Testing")]

namespace irods_Csharp;

public abstract class Message
{
}

#region General

[XmlType("MsgHeader_PI")]
public class MsgHeaderPi : Message
{
    [XmlElement("type")]
    public string Type { get; set; }

    [XmlElement("msgLen")]
    public int MsgLen { get; set; }

    [XmlElement("errorLen")]
    public int ErrorLen { get; set; }

    [XmlElement("bsLen")]
    public int BsLen { get; set; }

    [XmlElement("intInfo")]
    public int IntInfo { get; set; }

    public MsgHeaderPi()
    {
    }

    public MsgHeaderPi(string type, int msgLen, int errorLen, int bsLen, int intInfo)
    {
        Type = type;
        MsgLen = msgLen;
        ErrorLen = errorLen;
        BsLen = bsLen;
        IntInfo = intInfo;
    }
}

[XmlType("RError_PI")]
public class RErrorPi : Message
{
    [XmlElement("count")]
    public int Count { get; set; }

    [XmlElement("RErrMsg_PI")]
    public RErrMsgPi[] RErrMsgPi { get; set; }

    public RErrorPi()
    {
    }

    public RErrorPi(int count, RErrMsgPi[] rErrMsgPi)
    {
        Count = count;
        RErrMsgPi = rErrMsgPi;
    }
}

[XmlType("RErrMsg_PI")]
public class RErrMsgPi : Message
{
    [XmlElement("status")]
    public int Status { get; set; }

    [XmlElement("msg")]
    public string Msg { get; set; }

    public RErrMsgPi()
    {
    }

    public RErrMsgPi(int status, string msg)
    {
        Status = status;
        Msg = msg;
    }
}

#endregion

#region Start

[XmlType("StartupPack_PI")]
public class StartupPackPi : Message
{
    [XmlElement("irodsProt")]
    public int IrodsProt { get; set; }

    [XmlElement("reconnFlag")]
    public int ReconnFlag { get; set; }

    [XmlElement("connectCnt")]
    public int ConnectCnt { get; set; }

    [XmlElement("proxyUser")]
    public string ProxyUser { get; set; }

    [XmlElement("proxyRcatZone")]
    public string ProxyRcatZone { get; set; }

    [XmlElement("clientUser")]
    public string ClientUser { get; set; }

    [XmlElement("clientRcatZone")]
    public string ClientRcatZone { get; set; }

    [XmlElement("relVersion")]
    public string RelVersion { get; set; }

    [XmlElement("apiVersion")]
    public string ApiVersion { get; set; }

    [XmlElement("option")]
    public string Option { get; set; }

    public StartupPackPi()
    {
    }

    public StartupPackPi(
        iRODSProt_t irodsProt,
        int reconnFlag,
        int connectCnt,
        string proxyUser,
        string proxyRcatZone,
        string clientUser,
        string clientRcatZone,
        string relVersion,
        string apiVersion,
        string option
    )
    {
        IrodsProt = (int)irodsProt;
        ReconnFlag = reconnFlag;
        ConnectCnt = connectCnt;
        ProxyUser = proxyUser;
        ProxyRcatZone = proxyRcatZone;
        ClientUser = clientUser;
        ClientRcatZone = clientRcatZone;
        RelVersion = relVersion;
        ApiVersion = apiVersion;
        Option = option;
    }
}

[XmlType("CS_NEG_PI")]
public class CsNegPi : Message
{
    [XmlElement("status")]
    public int Status { get; set; }

    [XmlElement("result")]
    public string Result { get; set; }

    public CsNegPi()
    {
    }

    public CsNegPi(ClientServerPolicyResult policyResult)
    {
        Status = policyResult is ClientServerPolicyResult.Failure ? 0 : 1;
        Result = $"cs_neg_result_kw={ClientServerPolicyToString(policyResult)};";
    }

    private static string ClientServerPolicyToString(ClientServerPolicyResult policyResult) =>
        policyResult switch
        {
            ClientServerPolicyResult.UseTCP => "CS_NEG_USE_TCP",
            ClientServerPolicyResult.UseSSL => "CS_NEG_USE_SSL",
            ClientServerPolicyResult.Failure => "CS_NEG_FAILURE",
            _ => throw new ArgumentOutOfRangeException(nameof(policyResult), policyResult, null)
        };

    public ClientServerPolicyRequest ServerPolicy =>
        Result switch
        {
            "CS_NEG_REQUIRE" => ClientServerPolicyRequest.RequireSSL,
            "CS_NEG_REFUSE" => ClientServerPolicyRequest.RefuseSSL,
            "CS_NEG_DONT_CARE" => ClientServerPolicyRequest.DontCare,
            _ => throw new ArgumentOutOfRangeException()
        };
}

[XmlType("Version_PI")]
public class VersionPi : Message
{
    [XmlElement("status")]
    public int Status { get; set; }

    [XmlElement("relVersion")]
    public string RelVersion { get; set; }

    [XmlElement("apiVersion")]
    public string ApiVersion { get; set; }

    [XmlElement("reconnPort")]
    public int ReconnPort { get; set; }

    [XmlElement("reconnAddr")]
    public string ReconnAddr { get; set; }

    [XmlElement("cookie")]
    public int Cookie { get; set; }

    public VersionPi()
    {
    }

    public VersionPi(int status, string relVersion, string apiVersion, int reconnPort, string reconnAddr, int cookie)
    {
        Status = status;
        RelVersion = relVersion;
        ApiVersion = apiVersion;
        ReconnPort = reconnPort;
        ReconnAddr = reconnAddr;
        Cookie = cookie;
    }
}

[XmlType("authPlugReqOut_PI")]
public class AuthPlugReqOutPi : Message
{
    [XmlElement("result_")]
    public string Result { get; set; }

    public AuthPlugReqOutPi()
    {
    }

    public AuthPlugReqOutPi(string result)
    {
        Result = result;
    }
}

[XmlType("authResponseInp_PI")]
public class AuthResponseInpPi : Message
{
    [XmlElement("response")]
    public string Response { get; set; }

    [XmlElement("username")]
    public string Username { get; set; }

    public AuthResponseInpPi()
    {
    }

    public AuthResponseInpPi(string response, string username)
    {
        Response = response;
        Username = username;
    }
}

[XmlType("authPlugReqInp_PI")]
public class AuthPlugReqInpPi : Message
{
    [XmlElement("auth_scheme_")]
    public string AuthScheme { get; set; }

    [XmlElement("context_")]
    public string Context { get; set; }

    public AuthPlugReqInpPi()
    {
    }

    public AuthPlugReqInpPi(string authScheme, string context)
    {
        AuthScheme = authScheme;
        Context = context;
    }
}

[XmlType("MiscSvrInfo_PI")]
public class MiscSvrInfoPi : Message
{
    [XmlElement("status")]
    public int Status { get; set; }

    [XmlElement("relVersion")]
    public string RelVersion { get; set; }

    [XmlElement("apiVersion")]
    public string ApiVersion { get; set; }

    [XmlElement("reconnPort")]
    public int ReconnPort { get; set; }

    [XmlElement("reconnAddr")]
    public string ReconnAddr { get; set; }

    [XmlElement("cookie")]
    public int Cookie { get; set; }

    public MiscSvrInfoPi()
    {
    }

    public MiscSvrInfoPi(
        int status,
        string relVersion,
        string apiVersion,
        int reconnPort,
        string reconnAddr,
        int cookie
    )
    {
        Status = status;
        RelVersion = relVersion;
        ApiVersion = apiVersion;
        ReconnPort = reconnPort;
        ReconnAddr = reconnAddr;
        Cookie = cookie;
    }
}

[XmlType("authRequestOut_PI")]
public class AuthRequestOutPi : Message
{
    [XmlElement("challenge")]
    public string Challenge { get; set; }

    public AuthRequestOutPi()
    {
    }

    public AuthRequestOutPi(string challenge)
    {
        Challenge = challenge;
    }
}

[XmlType("sslStartInp_PI")]
public class SSLStartInpPi : Message
{
    [XmlElement("arg0")]
    public string Arg0 { get; set; }

    public SSLStartInpPi()
    {
    }

    public SSLStartInpPi(string arg0)
    {
        Arg0 = arg0;
    }
}

#endregion

#region DataObj

[XmlType("DataObjInp_PI")]
public class DataObjInpPi : Message
{
    [XmlElement("objPath")]
    public string ObjPath { get; set; }

    [XmlElement("createMode")]
    public int CreateMode { get; set; }

    [XmlElement("openFlags")]
    public int OpenFlags { get; set; }

    [XmlElement("offset")]
    public double Offset { get; set; }

    [XmlElement("dataSize")]
    public double DataSize { get; set; }

    [XmlElement("numThreads")]
    public int NumThreads { get; set; }

    [XmlElement("oprType")]
    public int OprType { get; set; }

    [XmlElement("KeyValPair_PI")]
    public KeyValPairPi KeyValPairPi { get; set; }

    public DataObjInpPi()
    {

    }

    public DataObjInpPi(
        string objPath,
        int createMode,
        int openFlags,
        double offset,
        double dataSize,
        int numThreads,
        int oprType,
        KeyValPairPi keyValPairPi = null
    )
    {
        ObjPath = objPath;
        CreateMode = createMode;
        OpenFlags = openFlags;
        Offset = offset;
        DataSize = dataSize;
        NumThreads = numThreads;
        OprType = oprType;
        KeyValPairPi = keyValPairPi;
    }
}

[XmlType("DataObjCopyInp_PI")]
public class DataObjCopyInpPi : Message
{
    [XmlElement("src")]
    public DataObjInpPi Src { get; set; }

    [XmlElement("dest")]
    public DataObjInpPi Dest { get; set; }

    public DataObjCopyInpPi()   
    {
    }

    public DataObjCopyInpPi(DataObjInpPi src = null, DataObjInpPi dest = null)
    {
        Src = src;
        Dest = dest;
    }
}

[XmlType("OpenedDataObjInp_PI")]
public class OpenedDataObjInpPi : Message
{
    [XmlElement("l1descInx")]
    public int L1descInx { get; set; }

    [XmlElement("len")]
    public int Len { get; set; }

    [XmlElement("whence")]
    public int Whence { get; set; }

    [XmlElement("oprType")]
    public int OprType { get; set; }

    [XmlElement("offset")]
    public double Offset { get; set; }

    [XmlElement("bytesWritten")]
    public double BytesWritten { get; set; }

    [XmlElement("KeyValPair_PI")]
    public KeyValPairPi KeyValPairPi { get; set; }

    public OpenedDataObjInpPi()
    {

    }

    public OpenedDataObjInpPi(
        int l1descInx,
        int len,
        int whence,
        int oprType,
        double offset,
        double bytesWritten,
        KeyValPairPi keyValPairPi = null
    )
    {
        L1descInx = l1descInx;
        Len = len;
        Whence = whence;
        OprType = oprType;
        Offset = offset;
        BytesWritten = bytesWritten;
        KeyValPairPi = keyValPairPi;
    }
}

[XmlType("fileLseekOut_PI")]
public class FileLseekOutPi : Message
{
    [XmlElement("offset")]
    public int Offset { get; set; }

    public FileLseekOutPi()
    {
    }
}

#endregion

#region Collection

[XmlType("CollInpNew_PI")]
public class CollInpNewPi : Message
{
    [XmlElement("collName")]
    public string CollName { get; set; }

    [XmlElement("flags")]
    public int Flags { get; set; }

    [XmlElement("oprType")]
    public int OprType { get; set; }

    [XmlElement("KeyValPair_PI")]
    public KeyValPairPi KeyValPairPi { get; set; }

    public CollInpNewPi()
    {
    }

    public CollInpNewPi(string collName, int flags, int oprType, KeyValPairPi keyValPairPi = null)
    {
        CollName = collName;
        Flags = flags;
        OprType = oprType;
        KeyValPairPi = keyValPairPi;
    }
}

[XmlType("CollOprStat_PI")]
public class CollOprStatPi : Message
{
    [XmlElement("filesCnt")]
    public int FilesCnt { get; set; }

    [XmlElement("totalFileCnt")]
    public int TotalFileCnt { get; set; }

    [XmlElement("bytesWritten")]
    public double BytesWritten { get; set; }

    [XmlElement("lastObjPath")]
    public string LastObjPath { get; set; }

    public CollOprStatPi()
    {
    }

    public CollOprStatPi(int filesCnt, int totalFileCnt, double bytesWritten, string lastObjPath)
    {
        FilesCnt = filesCnt;
        TotalFileCnt = totalFileCnt;
        BytesWritten = bytesWritten;
        LastObjPath = lastObjPath;
    }
}

#endregion

#region Query

[XmlType("GenQueryInp_PI")]
public class GenQueryInpPi : Message
{
    [XmlElement("maxRows")]
    public int MaxRows { get; set; }

    [XmlElement("continueInx")]
    public int ContinueInx { get; set; }

    [XmlElement("partialStartIndex")]
    public int PartialStartIndex { get; set; }

    [XmlElement("options")]
    public int Options { get; set; }

    [XmlElement("KeyValPair_PI")]
    public KeyValPairPi KeyValPairPi { get; set; }

    [XmlElement("InxIvalPair_PI")]
    public InxIvalPairPi InxIvalPairPi { get; set; }

    [XmlElement("InxValPair_PI")]
    public InxValPairPi InxValPairPi { get; set; }

    public GenQueryInpPi()
    {

    }

    public GenQueryInpPi(
        int maxRows,
        int continueInx,
        int partialStartIndex,
        int options,
        KeyValPairPi keyValPairPi,
        InxIvalPairPi inxIvalPairPi,
        InxValPairPi inxValPairPi
    )
    {
        MaxRows = maxRows;
        ContinueInx = continueInx;
        PartialStartIndex = partialStartIndex;
        Options = options;
        KeyValPairPi = keyValPairPi;
        InxIvalPairPi = inxIvalPairPi;
        InxValPairPi = inxValPairPi;
    }
}

[XmlType("GenQueryOut_PI")]
public class GenQueryOutPi : Message
{
    [XmlElement("rowCnt")]
    public int RowCnt { get; set; }

    [XmlElement("attriCnt")]
    public int AttriCnt { get; set; }

    [XmlElement("continueInx")]
    public int ContinueInx { get; set; }

    [XmlElement("totalRowCount")]
    public int TotalRowCount { get; set; }

    [XmlElement("SqlResult_PI")]
    public SqlResultPi[] SqlResultPi { get; set; }

    public GenQueryOutPi()
    {
    }

    public GenQueryOutPi(int rowCnt, int attriCnt, int continueInx, int totalRowCount, SqlResultPi[] sqlResultPi)
    {
        RowCnt = rowCnt;
        AttriCnt = attriCnt;
        ContinueInx = continueInx;
        TotalRowCount = totalRowCount;
        SqlResultPi = sqlResultPi;
    }
}

[XmlType("SqlResult_PI")]
public class SqlResultPi : Message
{
    [XmlElement("attriInx")]
    public int AttriInx { get; set; }

    [XmlElement("reslen")]
    public int Reslen { get; set; }

    [XmlElement("value")]
    public string[] Value { get; set; }

    public SqlResultPi()
    {
    }

    public SqlResultPi(int attriInx, int reslen, string[] value)
    {
        AttriInx = attriInx;
        Reslen = reslen;
        Value = value;
    }
}

#endregion

#region Meta

[XmlType("ModAVUMetadataInp_PI")]
public class ModAvuMetadataInpPi : Message
{
    public string Arg0, Arg1, Arg2, Arg3, Arg4, Arg5;
    public string? Arg6, Arg7, Arg8, Arg9;

    public ModAvuMetadataInpPi()
    {

    }

    public ModAvuMetadataInpPi(string mode, string type, string path, string name, string value, int units)
    {
        Arg0 = mode;
        Arg1 = type;
        Arg2 = path;
        Arg3 = name;
        Arg4 = value;
        Arg5 = units == -1 ? "" : units.ToString();
        Arg6 = null;
        Arg7 = null;
        Arg8 = null;
        Arg9 = null;
    }
}

#endregion

#region ValPairs

[XmlType("KeyValPair_PI")]
public class KeyValPairPi : Message
{
    [XmlElement("ssLen")]
    public int SsLen { get; set; }

    [XmlElement("keyWord")]
    public string[]? KeyWord { get; set; }

    [XmlElement("svalue")]
    public string[]? Svalue { get; set; }

    public KeyValPairPi()
    {
    }

    public KeyValPairPi(int ssLen, string[]? keyWord, string[]? svalue)
    {
        SsLen = ssLen;
        KeyWord = keyWord;
        Svalue = svalue;
    }
}

[XmlType("InxIvalPair_PI")]
public class InxIvalPairPi : Message
{
    [XmlElement("iiLen")]
    public int IiLen { get; set; }

    [XmlElement("inx")]
    public int[]? Inx { get; set; }

    [XmlElement("ivalue")]
    public int[]? Ivalue { get; set; }

    public InxIvalPairPi(int iiLen, int[]? inx, int[]? ivalue)
    {
        IiLen = iiLen;
        Inx = inx;
        Ivalue = ivalue;
    }

    public InxIvalPairPi()
    {

    }

    public override string ToString()
    {
        StringBuilder sb = new ();
        sb.AppendLine($"<{GetType().Name}>");
        sb.AppendLine($"<iiLen>{IiLen}</iiLen>");
        if (Inx != null)
            foreach (int inx in Inx)
                sb.AppendLine($"<inx>{inx}</inx>");
        if (Ivalue != null)
            foreach (int ivalue in Ivalue)
                sb.AppendLine($"<ivalue>{ivalue}</ivalue>");
        sb.Append($"</{GetType().Name}>");
        return sb.ToString();
    }
}

[XmlType("InxValPair_PI")]
public class InxValPairPi : Message
{
    [XmlElement("isLen")]
    public int IsLen { get; set; }

    [XmlElement("inx")]
    public int[]? Inx { get; set; }

    [XmlElement("svalue")]
    public string[]? Svalue { get; set; }

    public InxValPairPi()
    {
    }

    public InxValPairPi(int isLen, int[]? inx, string[]? svalue)
    {
        IsLen = isLen;
        Inx = inx;
        Svalue = svalue;
    }
}

#endregion

#region Unused

[XmlType("SpecColl_PI")]
public class SpecCollPi : Message
{
    [XmlElement("collClass")]
    public int CollClass { get; set; }

    [XmlElement("type")]
    public int Type { get; set; }

    [XmlElement("collection")]
    public string Collection { get; set; }

    [XmlElement("objPath")]
    public string ObjPath { get; set; }

    [XmlElement("resource")]
    public string Resource { get; set; }

    [XmlElement("rescHier")]
    public string RescHier { get; set; }

    [XmlElement("phyPath")]
    public string PhyPath { get; set; }

    [XmlElement("cacheDir")]
    public string CacheDir { get; set; }

    [XmlElement("cacheDirty")]
    public int CacheDirty { get; set; }

    [XmlElement("replNum")]
    public int ReplNum { get; set; }

    public SpecCollPi(
        int collClass,
        int type,
        string collection,
        string objPath,
        string resource,
        string rescHier,
        string phyPath,
        string cacheDir,
        int cacheDirty,
        int replNum
    )
    {
        CollClass = collClass;
        Type = type;
        Collection = collection;
        ObjPath = objPath;
        Resource = resource;
        RescHier = rescHier;
        PhyPath = phyPath;
        CacheDir = cacheDir;
        CacheDirty = cacheDirty;
        ReplNum = replNum;
    }
}

#endregion