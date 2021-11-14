namespace DotNetDBTools.Deploy.MySQL
{
    internal enum MySQLDbObjectsTypes
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
