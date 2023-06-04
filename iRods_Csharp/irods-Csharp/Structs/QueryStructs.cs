// ReSharper disable InconsistentNaming
namespace irods_Csharp;

/// <summary>
/// Class for conditions in the "where" part of a query
/// </summary>
public class Condition
{
    public Column Column;
    protected string op;
    protected string value;

    public Condition(Column column, string op, string value)
    {
        Column = column;
        this.op = op;
        this.value = value;
    }

    public override string ToString()
    {
        return op + " '" + value + "'";
    }
}

/// <summary>
/// Class for all table columns which can currently be queried
/// </summary>
public struct Column
{
    public SqlType Type;
    public string Key;
    public int Id;

    public Column(SqlType type, string key, int id)
    {
        Type = type;
        Key = key;
        Id = id;
    }
}

/// <summary>
/// Types within ICAT table
/// </summary>
public enum SqlType
{
    String,
    Integer,
    DateTime
}

/// <summary>
/// Models for all things that can be queried
/// <summary>
public static class QueryModels 
{
    /// <summary>
    /// Columns belonging to data objects
    /// </summary>
    /// <returns>Array of columns belonging to data objects</returns>
    public static Column[] DataObject() =>
        new []
        {
            D_DATA_ID, D_COLL_ID, DATA_NAME, DATA_REPL_NUM, DATA_VERSION, DATA_TYPE_NAME, DATA_SIZE, 
            D_RESC_NAME, D_DATA_PATH, D_OWNER_NAME, D_OWNER_ZONE, D_REPL_STATUS, D_DATA_STATUS, D_DATA_CHECKSUM,
            D_EXPIRY, D_MAP_ID, D_COMMENTS, D_CREATE_TIME, D_MODIFY_TIME
        };

    /// <summary>
    /// Columns belonging to collections
    /// </summary>
    /// <returns>Array of columns belonging to collections</returns>
    public static Column[] Collection() =>
        new[]
        {
            COLL_ID, COLL_NAME, COLL_PARENT_NAME, COLL_OWNER_NAME, COLL_OWNER_ZONE, COLL_MAP_ID, COLL_INHERITANCE,
            COLL_COMMENTS, COLL_CREATE_TIME, COLL_MODIFY_TIME
        };

    /// <summary>
    /// Columns belonging to data object metadata
    /// </summary>
    /// <returns>Array of columns belonging to data object metadata</returns>
    public static Column[] DataObjMeta() =>
        new[]
        {
            COL_META_DATA_ATTR_NAME, COL_META_DATA_ATTR_VALUE, COL_META_DATA_ATTR_UNITS, COL_META_DATA_ATTR_ID, COL_META_DATA_CREATE_TIME, COL_META_DATA_MODIFY_TIME
        };

    /// <summary>
    /// Columns belonging to collection metadata
    /// </summary>
    /// <returns>Array of columns belonging to collection metadata</returns>
    public static Column[] CollectionMeta() =>
        new[]
        {
            COL_META_COLL_ATTR_NAME, COL_META_COLL_ATTR_VALUE, COL_META_COLL_ATTR_UNITS, COL_META_COLL_ATTR_ID, COL_META_COLL_CREATE_TIME, COL_META_COLL_MODIFY_TIME
        };

    public static Column D_DATA_ID = new (SqlType.Integer, "D_DATA_ID", 401);
    public static Column D_COLL_ID = new (SqlType.Integer, "D_COLL_ID", 402);
    public static Column DATA_NAME = new (SqlType.String, "DATA_NAME", 403);
    public static Column DATA_REPL_NUM = new (SqlType.Integer, "DATA_REPL_NUM", 404);
    public static Column DATA_VERSION = new (SqlType.String, "DATA_VERSION", 405);
    public static Column DATA_TYPE_NAME = new (SqlType.String, "DATA_TYPE_NAME", 406);
    public static Column DATA_SIZE = new (SqlType.Integer, "DATA_SIZE", 407);
    public static Column D_RESC_NAME = new (SqlType.String, "D_RESC_NAME", 409);
    public static Column D_DATA_PATH = new (SqlType.String, "D_DATA_PATH", 410);
    public static Column D_OWNER_NAME = new (SqlType.String, "D_OWNER_NAME", 411);
    public static Column D_OWNER_ZONE = new (SqlType.String, "D_OWNER_ZONE", 412);
    public static Column D_REPL_STATUS = new (SqlType.String, "D_REPL_STATUS", 413);
    public static Column D_DATA_STATUS = new (SqlType.String, "D_DATA_STATUS", 414);
    public static Column D_DATA_CHECKSUM = new (SqlType.String, "D_DATA_CHECKSUM", 415);
    public static Column D_EXPIRY = new (SqlType.String, "D_EXPIRY", 416);
    public static Column D_MAP_ID = new (SqlType.Integer, "D_MAP_ID", 417);
    public static Column D_COMMENTS = new (SqlType.String, "D_COMMENTS", 418);
    public static Column D_CREATE_TIME = new (SqlType.DateTime, "D_CREATE_TIME", 419);
    public static Column D_MODIFY_TIME = new (SqlType.DateTime, "D_MODIFY_TIME", 420);

    public static Column COLL_ID = new (SqlType.Integer, "COLL_ID", 500);
    public static Column COLL_NAME = new (SqlType.String, "COLL_NAME", 501);
    public static Column COLL_PARENT_NAME = new (SqlType.String, "COLL_PARENT_NAME", 502);
    public static Column COLL_OWNER_NAME = new (SqlType.String, "COLL_OWNER_NAME", 503);
    public static Column COLL_OWNER_ZONE = new (SqlType.String, "COLL_OWNER_ZONE", 504);
    public static Column COLL_MAP_ID = new (SqlType.String, "COLL_MAP_ID", 505);
    public static Column COLL_INHERITANCE =new (SqlType.String, "COLL_INHERITANCE", 506);
    public static Column COLL_COMMENTS = new (SqlType.String, "COLL_COMMENTS", 507);
    public static Column COLL_CREATE_TIME = new (SqlType.DateTime, "COLL_CREATE_TIME", 508);
    public static Column COLL_MODIFY_TIME = new (SqlType.DateTime, "COLL_MODIFY_TIME", 509);

    public static Column COL_META_DATA_ATTR_NAME = new (SqlType.String, "COL_META_DATA_ATTR_NAME", 600);
    public static Column COL_META_DATA_ATTR_VALUE = new (SqlType.String, "COL_META_DATA_ATTR_VALUE", 601);
    public static Column COL_META_DATA_ATTR_UNITS = new (SqlType.String, "COL_META_DATA_ATTR_UNITS", 602);
    public static Column COL_META_DATA_ATTR_ID = new (SqlType.String, "COL_META_DATA_ATTR_ID", 603);
    public static Column COL_META_DATA_CREATE_TIME = new (SqlType.DateTime, "COL_META_DATA_CREATE_TIME", 604);
    public static Column COL_META_DATA_MODIFY_TIME = new (SqlType.DateTime, "COL_META_DATA_MODIFY_TIME", 605);

    public static Column COL_META_COLL_ATTR_NAME = new (SqlType.String, "COL_META_COLL_ATTR_NAME", 610);
    public static Column COL_META_COLL_ATTR_VALUE = new (SqlType.String, "COL_META_COLL_ATTR_VALUE", 611);
    public static Column COL_META_COLL_ATTR_UNITS = new (SqlType.String, "COL_META_COLL_ATTR_UNITS", 612);
    public static Column COL_META_COLL_ATTR_ID = new (SqlType.String, "COL_META_COLL_ATTR_ID", 613);
    public static Column COL_META_COLL_CREATE_TIME = new (SqlType.DateTime, "COL_META_COLL_CREATE_TIME", 614);
    public static Column COL_META_COLL_MODIFY_TIME = new (SqlType.DateTime, "COL_META_COLL_MODIFY_TIME", 615);
}