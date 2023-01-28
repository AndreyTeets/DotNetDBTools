namespace DotNetDBTools.Models.PostgreSQL;

public static class PostgreSQLIndexMethods
{
    public const string BTREE = nameof(BTREE);
    public const string HASH = nameof(HASH);
    public const string GIST = nameof(GIST);
    public const string SPGIST = nameof(SPGIST);
    public const string GIN = nameof(GIN);
    public const string BRIN = nameof(BRIN);
}
