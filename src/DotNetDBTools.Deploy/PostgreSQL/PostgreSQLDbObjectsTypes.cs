namespace DotNetDBTools.Deploy.PostgreSQL
{
    internal enum PostgreSQLDbObjectsTypes
    {
        Table,
        Column,
        PrimaryKey,
        UniqueConstraint,
        CheckConstraint,
        Index,
        Trigger,
        ForeignKey,
        View,
        Function,
        Procedure,
    }
}
