namespace DotNetDBTools.Models.Core
{
    public static class ForeignKeyActions
    {
        public const string NoAction = "NO ACTION";
        public const string Restrict = "RESTRICT";
        public const string Cascade = "CASCADE";
        public const string SetDefault = "SET DEFAULT";
        public const string SetNull = "SET NULL";
    }
}
