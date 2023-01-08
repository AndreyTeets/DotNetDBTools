namespace DotNetDBTools.Models.PostgreSQL;

public static class PostgreSQLDataTypeNames
{
    public const string SMALLINT = nameof(SMALLINT);
    public const string INT = nameof(INT);
    public const string BIGINT = nameof(BIGINT);

    public const string FLOAT4 = nameof(FLOAT4);
    public const string FLOAT8 = nameof(FLOAT8);
    public const string DECIMAL = nameof(DECIMAL);
    public const string BOOL = nameof(BOOL);

    public const string MONEY = nameof(MONEY);

    public const string CHAR = nameof(CHAR);
    public const string VARCHAR = nameof(VARCHAR);
    public const string TEXT = nameof(TEXT);

    public const string BYTEA = nameof(BYTEA);

    public const string DATE = nameof(DATE);
    public const string TIME = nameof(TIME);
    public const string TIMETZ = nameof(TIMETZ);
    public const string TIMESTAMP = nameof(TIMESTAMP);
    public const string TIMESTAMPTZ = nameof(TIMESTAMPTZ);
    public const string INTERVAL = nameof(INTERVAL);

    public const string UUID = nameof(UUID);
    public const string BIT = nameof(BIT);
    public const string VARBIT = nameof(VARBIT);
    public const string JSON = nameof(JSON);
    public const string JSONB = nameof(JSONB);
    public const string XML = nameof(XML);

    public const string TSQUERY = nameof(TSQUERY);
    public const string TSVECTOR = nameof(TSVECTOR);

    public const string POINT = nameof(POINT);
    public const string LINE = nameof(LINE);
    public const string LSEG = nameof(LSEG);
    public const string BOX = nameof(BOX);
    public const string PATH = nameof(PATH);
    public const string POLYGON = nameof(POLYGON);
    public const string CIRCLE = nameof(CIRCLE);

    public const string INET = nameof(INET);
    public const string CIDR = nameof(CIDR);
    public const string MACADDR = nameof(MACADDR);
    public const string MACADDR8 = nameof(MACADDR8);
}
