namespace DotNetDBTools.Definition.PostgreSQL
{
    public enum ForeignKeyActions
    {
        NoAction,
        Restrict,
        Cascade,
        SetNull,
        SetDefault,
    }
}
