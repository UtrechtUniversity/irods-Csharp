using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
// ReSharper disable EmptyConstructor
// ReSharper disable NotAccessedField.Local
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

namespace irods_Csharp;

internal abstract class IRodsMessage
{
    /// <summary>
    /// Turns the object into xml form and encodes this using the UTF8 format.
    /// </summary>
    /// <returns>The byte[] containing the serialized object.</returns>
    public byte[] Serialize() => Encoding.UTF8.GetBytes(ToString());

    /// <summary>
    /// Turns the object into a XML string.
    /// </summary>
    /// <returns>The string in XML format.</returns>
    public override string ToString()
    {
        string result = string.Empty;
        result += $"<{GetType().Name}>\n";
        foreach (FieldInfo Field in GetType().GetFields())
        {
            switch (Field.GetValue(this))
            {
                case object[] value:
                    result += $"<{Field.Name}>[";
                    for (int i = 0; i < value.Length; i++)
                    {
                        result += value[i];
                        if (i < value.Length - 1) result += ",";
                    }

                    result += $"]</{Field.Name}>\n";
                    break;
                case IRodsMessage message:
                    result += $"{message}\n";
                    break;
                case { } value:
                    result += $"<{Field.Name}>{value}</{Field.Name}>\n";
                    break;
            }
        }
        result += $"</{GetType().Name}>";
        return result;
    }

    /// <summary>
    /// Deserializes the given byte[] into a IRodsMessage.
    /// </summary>
    /// <typeparam name="T">The type of the deserialized IRodsMessage.</typeparam>
    /// <param name="bytes">The byte[] to deserialize.</param>
    /// <returns>The deserialized object.</returns>
    internal static T Deserialize<T>(byte[] bytes) where T : IRodsMessage, new()
    {
        string a = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
        return (T)Parse(a);
    }

    /// <summary>
    /// Turns a XML string into a IRodsMessage.
    /// </summary>
    /// <param name="content">The string to turn into an IRodsMessage.</param>
    /// <returns>The IRodsMessage created with the string.</returns>
    internal static IRodsMessage Parse(string content)
    {
        string[] lines = content.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

        IRodsMessage message = (IRodsMessage)Activator.CreateInstance(Type.GetType("irods_Csharp." + lines[0][1..^1])!)!;

        for (int i = 1; i < lines.Length - 1; i++)
        {
            string[] d = lines[i].Split(new[] { "</", "<", ">" }, StringSplitOptions.RemoveEmptyEntries);

            object value;
            Type type = message.GetType().GetField(d[0])!.FieldType;
            if (type == typeof(int)) value = int.Parse(d[1]);
            else if (type == typeof(string)) value = d[1];
            else if (type == typeof(double)) value = double.Parse(d[1]);
            else if (type == typeof(IRodsMessage)) value = Parse(d[1]);
            else if (type == typeof(SqlResult_PI[])) value = DecodeSqlResult(lines, ref i);
            else throw new Exception($"Type: {type.Name} not handled in deserialize.");

            message.GetType().GetField(d[0])!.SetValue(message, value);
        }
        return message;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="lines"></param>
    /// <param name="i"></param>
    /// <returns></returns>
    public static SqlResult_PI[] DecodeSqlResult(string[] lines, ref int i)
    {
        List<SqlResult_PI> results = new ();
        while(lines[i] != "</GenQueryOut_PI>")
        {
            i++;
            int attriInx = int.Parse(lines[i].Substring(10, lines[i].Length - 21));
            int reslen = int.Parse(lines[i+1].Substring(8, lines[i+1].Length - 17));
            i += 2;
            List<string> values = new ();
            while (lines[i] != "</SqlResult_PI>")
            {
                values.Add(lines[i].Substring(7, lines[i].Length - 15));
                i++;
            }
            results.Add(new SqlResult_PI(attriInx, reslen, values.ToArray()));
            i++;
        }
        return results.ToArray();
    }
}

internal class Packet<T> where T : IRodsMessage
{
    public MsgHeader_PI MsgHeader;
    public T? MsgBody;
    public RError_PI? Error;
    public byte[]? Binary;
        
    public Packet() { }

    public Packet(int intInfo = 0, string type = MessageType.API_REQ)
    {
        MsgHeader = new MsgHeader_PI(type, 0, 0, 0, intInfo);
    }

    public override string ToString()
    {
        string result = "";
        foreach (FieldInfo Field in GetType().GetFields()) if (Field.GetValue(this) != null) result += Field.GetValue(this) + "\n";
        return result;
    }
}

#region General
internal class MsgHeader_PI : IRodsMessage
{
    public string type;
    public int msgLen;
    public int errorLen;
    public int bsLen;
    public int intInfo;

    public MsgHeader_PI() { }

    public MsgHeader_PI(string type, int msgLen, int errorLen, int bsLen, int intInfo)
    {
        this.type = type;
        this.msgLen = msgLen;
        this.errorLen = errorLen;
        this.bsLen = bsLen;
        this.intInfo = intInfo;
    }
}

internal class RError_PI : IRodsMessage
{
    public int count;
    public RErrMsg_PI[] RErrMsg_PI;

    public RError_PI() { }

    public RError_PI(int count, RErrMsg_PI[] RErrMsg_PI)
    {
        this.count = count;
        this.RErrMsg_PI = RErrMsg_PI;
    }
}

internal class RErrMsg_PI : IRodsMessage
{
    public int status;
    public string msg;

    public RErrMsg_PI(int status, string msg)
    {
        this.status = status;
        this.msg = msg;
    }
}

internal class None : IRodsMessage
{
    public None() { }
}
#endregion

#region Start
internal class StartupPack_PI : IRodsMessage
{
    public int irodsProt;
    public int reconnFlag;
    public int connectCnt;
    public string proxyUser;
    public string proxyRcatZone;
    public string clientUser;
    public string clientRcatZone;
    public string relVersion;
    public string apiVersion;
    public string option;

    public StartupPack_PI() { }

    public StartupPack_PI(Options.iRODSProt_t irodsProt, int reconnFlag, int connectCnt, string proxyUser, string proxyRcatZone, string clientUser, string clientRcatZone, string relVersion, string apiVersion, string option)
    {
        this.irodsProt = (int)irodsProt;
        this.reconnFlag = reconnFlag;
        this.connectCnt = connectCnt;
        this.proxyUser = proxyUser;
        this.proxyRcatZone = proxyRcatZone;
        this.clientUser = clientUser;
        this.clientRcatZone = clientRcatZone;
        this.relVersion = relVersion;
        this.apiVersion = apiVersion;
        this.option = option;
    }
}

internal class CS_NEG_PI : IRodsMessage
{
    public int status;
    public string result;

    public CS_NEG_PI() { }

    public CS_NEG_PI(ClientServerPolicyResult policyResult)
    {
        status = policyResult is ClientServerPolicyResult.failure ? 0 : 1;
        result = $"CS_NEG_RESULT_KW={ClientServerPolicyToString(policyResult)}";
    }

    private static string ClientServerPolicyToString(ClientServerPolicyResult policyResult) =>
        policyResult switch
        {
            ClientServerPolicyResult.useTCP => "CS_NEG_USE_TCP",
            ClientServerPolicyResult.useSSL => "CS_NEG_USE_SSL",
            ClientServerPolicyResult.failure => "CS_NEG_FAILURE",
            _ => throw new ArgumentOutOfRangeException(nameof(policyResult), policyResult, null)
        };

    public ClientServerPolicyRequest ServerPolicy =>
        result switch
        {
            "CS_NEG_REQUIRE" => ClientServerPolicyRequest.RequireSSL,
            "CS_NEG_REFUSE" => ClientServerPolicyRequest.RefuseSSL,
            _ => throw new ArgumentOutOfRangeException()
        };
}

internal class Version_PI : IRodsMessage
{
    public int status;
    public string relVersion;
    public string apiVersion;
    public int reconnPort;
    public string reconnAddr;
    public int cookie;

    public Version_PI() { }

    public Version_PI(int status, string relVersion, string apiVersion, int reconnPort, string reconnAddr, int cookie)
    {
        this.status = status;
        this.relVersion = relVersion;
        this.apiVersion = apiVersion;
        this.reconnPort = reconnPort;
        this.reconnAddr = reconnAddr;
        this.cookie = cookie;
    }
}

internal class authPlugReqOut_PI : IRodsMessage
{
    public string result_;

    public authPlugReqOut_PI() { }

    public authPlugReqOut_PI(string result_)
    {
        this.result_ = result_;
    }
}
internal class authResponseInp_PI : IRodsMessage
{
    public string response;
    public string username;

    public authResponseInp_PI() { }

    public authResponseInp_PI(string response, string username)
    {
        this.response = response;
        this.username = username;
    }
}

internal class authPlugReqInp_PI : IRodsMessage
{
    public string auth_scheme_;
    public string context_;

    public authPlugReqInp_PI() { }

    public authPlugReqInp_PI(string auth_scheme_, string context_)
    {
        this.auth_scheme_ = auth_scheme_;
        this.context_ = context_;
    }
}

internal class MiscSvrInfo_PI : IRodsMessage
{
    public int status;
    public string relVersion;
    public string apiVersion;
    public int reconnPort;
    public string reconnAddr;
    public int cookie;

    public MiscSvrInfo_PI() { }

    public MiscSvrInfo_PI(int status, string relVersion, string apiVersion, int reconnPort, string reconnAddr, int cookie)
    {
        this.status = status;
        this.relVersion = relVersion;
        this.apiVersion = apiVersion;
        this.reconnPort = reconnPort;
        this.reconnAddr = reconnAddr;
        this.cookie = cookie;
    }
}

internal class authRequestOut_PI : IRodsMessage
{
    public string challenge;

    public authRequestOut_PI() { }

    public authRequestOut_PI(string challenge)
    {
        this.challenge = challenge;
    }
}

internal class sslStartInp_PI : IRodsMessage
{
    public string arg0;
    public sslStartInp_PI() { }

    public sslStartInp_PI(string arg0)
    {
        this.arg0 = arg0;
    }
}
#endregion

#region DataObj
internal class DataObjInp_PI : IRodsMessage
{
    public string objPath;
    public int createMode;
    public int openFlags;
    public double offset;
    public double dataSize;
    public int numThreads;
    public int oprType;
    public KeyValPair_PI KeyValPair_PI;

    public DataObjInp_PI(string objPath, int createMode, int openFlags, double offset, double dataSize, int numThreads, int oprType, KeyValPair_PI KeyValPair_PI = null)
    {
        this.objPath = objPath;
        this.createMode = createMode;
        this.openFlags = openFlags;
        this.offset = offset;
        this.dataSize = dataSize;
        this.numThreads = numThreads;
        this.oprType = oprType;
        this.KeyValPair_PI = KeyValPair_PI;
    }
}

internal class DataObjCopyInp_PI : IRodsMessage
{
    public DataObjInp_PI src;
    public DataObjInp_PI dest;

    public DataObjCopyInp_PI(DataObjInp_PI src = null, DataObjInp_PI dest = null)
    {
        this.src = src;
        this.dest = dest;
    }
}

internal class OpenedDataObjInp_PI : IRodsMessage
{
    public int l1descInx;
    public int len;
    public int whence;
    public int oprType;
    public double offset;
    public double bytesWritten;
    public KeyValPair_PI KeyValPair_PI;

    public OpenedDataObjInp_PI(int l1descInx, int len, int whence, int oprType, double offset, double bytesWritten, KeyValPair_PI KeyValPair_PI = null)
    {
        this.l1descInx = l1descInx;
        this.len = len;
        this.whence = whence;
        this.oprType = oprType;
        this.offset = offset;
        this.bytesWritten = bytesWritten;
        this.KeyValPair_PI = KeyValPair_PI;
    }
}
internal class fileLseekOut_PI : IRodsMessage
{
    public int offset;

    public fileLseekOut_PI() { }
}
#endregion

#region Collection
internal class CollInpNew_PI : IRodsMessage
{
    public string collName;
    public int flags;
    public int oprType;
    public KeyValPair_PI KeyValPair_PI;

    public CollInpNew_PI() { }

    public CollInpNew_PI(string collName, int flags, int oprType, KeyValPair_PI KeyValPair_PI = null)
    {
        this.collName = collName;
        this.flags = flags;
        this.oprType = oprType;
        this.KeyValPair_PI = KeyValPair_PI;
    }
}

internal class CollOprStat_PI : IRodsMessage
{
    public int filesCnt;
    public int totalFileCnt;
    public double bytesWritten;
    public string lastObjPath;

    public CollOprStat_PI() { }

    public CollOprStat_PI(int filesCnt, int totalFileCnt, double bytesWritten, string lastObjPath)
    {
        this.filesCnt = filesCnt;
        this.totalFileCnt = totalFileCnt;
        this.bytesWritten = bytesWritten;
        this.lastObjPath = lastObjPath;
    }
}
#endregion

#region Query
internal class GenQueryInp_PI : IRodsMessage
{
    public int maxRows;
    public int continueInx;
    public int partialStartIndex;
    public int options;
    public KeyValPair_PI KeyValPair_PI;
    public InxIvalPair_PI InxIvalPair_PI;
    public InxValPair_PI InxValPair_PI;

    public GenQueryInp_PI(int maxRows, int continueInx, int partialStartIndex, int options, KeyValPair_PI KeyValPair_PI, InxIvalPair_PI InxIvalPair_PI, InxValPair_PI InxValPair_PI)
    {
        this.maxRows = maxRows;
        this.continueInx = continueInx;
        this.partialStartIndex = partialStartIndex;
        this.options = options;
        this.KeyValPair_PI = KeyValPair_PI;
        this.InxIvalPair_PI = InxIvalPair_PI;
        this.InxValPair_PI = InxValPair_PI;
    }
}

internal class GenQueryOut_PI : IRodsMessage
{
    public int rowCnt;
    public int attriCnt;
    public int continueInx;
    public int totalRowCount;
    public SqlResult_PI[] SqlResult_PI;

    public GenQueryOut_PI() { }

    public GenQueryOut_PI(int rowCnt, int attriCnt, int continueInx, int totalRowCount, SqlResult_PI[] sqlResult_PI)
    {
        this.rowCnt = rowCnt;
        this.attriCnt = attriCnt;
        this.continueInx = continueInx;
        this.totalRowCount = totalRowCount;
        SqlResult_PI = sqlResult_PI;
    }

    internal object Parse(Type type, IrodsSession session, Path home, Path path, Options.FileMode mode = Options.FileMode.ReadWrite, bool truncate = false)
    {
        if (type == typeof(Collection))
        {
            Collection[] collections = new Collection[rowCnt];
            const int collectionNameColumn = 1, collectionIdColumn = 0;

            for (int i = 0; i < rowCnt; i++)
            {
                Collection collection = new (new Path(SqlResult_PI[collectionNameColumn].value[i].Replace(home.ToString(), "")), int.Parse(SqlResult_PI[collectionIdColumn].value[i]), session.Collections);
                collections[i] = collection;
            }
            return collections;
        }
        if (type == typeof(DataObj))
        {
            List<DataObj> objects = new ();
            const int ObjNameColumn = 2;

            HashSet<string> names = new ();

            for (int i = 0; i < rowCnt; i++)
            {
                string name = SqlResult_PI[ObjNameColumn].value[i];
                if (names.Add(name))
                {
                    DataObj dataObj = session.DataObjects.Open(path + name, mode, truncate);
                    objects.Add(dataObj);
                }
            }
            return objects.ToArray();
        }
        if (type == typeof(Meta))
        {
            Meta[] objects = new Meta[rowCnt];
            const int metaNameColumn = 0, metaKeywordColumn = 1, metaUnitsColumn = 2;

            for (int i = 0; i < rowCnt; i++)
            {
                string unitValue = SqlResult_PI[metaUnitsColumn].value[i];
                int? units = unitValue == "" ? null : (int?)int.Parse(unitValue);
                Meta meta = new (SqlResult_PI[metaNameColumn].value[i], SqlResult_PI[metaKeywordColumn].value[i], units);
                objects[i] = meta;
            }
            return objects;
        }
        throw new Exception("Unknown Type");
    }
}

internal class SqlResult_PI : IRodsMessage
{
    public int attriInx;
    public int reslen;
    public string[] value;

    public SqlResult_PI() { }

    public SqlResult_PI(int attriInx, int reslen, string[] value)
    {
        this.attriInx = attriInx;
        this.reslen = reslen;
        this.value = value;
    }
}
#endregion

#region Meta
internal class ModAVUMetadataInp_PI : IRodsMessage
{
    public string arg0, arg1, arg2, arg3, arg4, arg5;
    public string? arg6, arg7, arg8, arg9;

    public ModAVUMetadataInp_PI(string mode, string type, string path, string name, string value, int units)
    {
        arg0 = mode;
        arg1 = type;
        arg2 = path;
        arg3 = name;
        arg4 = value;
        arg5 = units == -1 ? "" : units.ToString();
        arg6 = null;
        arg7 = null;
        arg8 = null;
        arg9 = null;
    }
}
#endregion

#region ValPairs
internal class KeyValPair_PI : IRodsMessage
{
    public int ssLen;
    public string[]? keyWord;
    public string[]? svalue;

    public KeyValPair_PI() { }

    public KeyValPair_PI(int ssLen, string[]? keyWord, string[]? svalue)
    {
        this.ssLen = ssLen;
        this.keyWord = keyWord;
        this.svalue = svalue;
    }

    public override string ToString()
    {
        StringBuilder sb = new ();
        sb.AppendLine($"<{GetType().Name}>");
        sb.AppendLine($"<ssLen>{ssLen}</ssLen>\n");
        if (keyWord != null) foreach (string keyword in keyWord) sb.AppendLine($"<keyWord>{keyword}</keyWord>");
        if (svalue != null) foreach (string value in svalue) sb.AppendLine($"<svalue>{value}</svalue>");
        sb.Append($"</{GetType().Name}>");
        return sb.ToString();
    }
}

internal class InxIvalPair_PI : IRodsMessage
{
    public int iiLen;
    public int[]? inx;
    public int[]? ivalue;

    public InxIvalPair_PI(int iiLen, int[]? inx, int[]? ivalue)
    {
        this.iiLen = iiLen;
        this.inx = inx;
        this.ivalue = ivalue;
    }

    public override string ToString()
    {
        StringBuilder sb = new ();
        sb.AppendLine($"<{GetType().Name}>");
        sb.AppendLine($"<iiLen>{iiLen}</iiLen>");
        if (inx != null) foreach (int inx in inx) sb.AppendLine( $"<inx>{inx}</inx>");
        if (ivalue != null) foreach (int ivalue in ivalue) sb.AppendLine($"<ivalue>{ivalue}</ivalue>");
        sb.Append( $"</{GetType().Name}>");
        return sb.ToString();
    }
}

internal class InxValPair_PI : IRodsMessage
{
    public int isLen;
    public int[]? inx;
    public string[]? svalue;

    public InxValPair_PI(int isLen, int[]? inx, string[]? svalue)
    {
        this.isLen = isLen;
        this.inx = inx;
        this.svalue = svalue;
    }

    public override string ToString()
    {
        StringBuilder sb = new ();
        sb.AppendLine($"<{GetType().Name}>");
        sb.AppendLine($"<isLen>{isLen}</isLen>");
        if (inx != null) foreach (int inx in inx) sb.AppendLine($"<inx>{inx}</inx>");
        if (svalue != null) foreach (string svalue in svalue) sb.AppendLine($"<svalue>{svalue}</svalue>");
        sb.Append($"</{GetType().Name}>");
        return sb.ToString();
    }
}
#endregion

#region Unused
internal class SpecColl_PI : IRodsMessage
{
    public int collClass;
    public int type;
    public string collection;
    public string objPath;
    public string resource;
    public string rescHier;
    public string phyPath;
    public string cacheDir;
    public int cacheDirty;
    public int replNum;

    public SpecColl_PI(int collClass, int type, string collection, string objPath, string resource, string rescHier, string phyPath, string cacheDir, int cacheDirty, int replNum)
    {
        this.collClass = collClass;
        this.type = type;
        this.collection = collection;
        this.objPath = objPath;
        this.resource = resource;
        this.rescHier = rescHier;
        this.phyPath = phyPath;
        this.cacheDir = cacheDir;
        this.cacheDirty = cacheDirty;
        this.replNum = replNum;
    }
}


#endregion