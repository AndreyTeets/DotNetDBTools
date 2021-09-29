namespace DotNetDBTools.Deploy.SQLite
{
    internal enum SQLiteDbObjectsTypes
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
    }
}
