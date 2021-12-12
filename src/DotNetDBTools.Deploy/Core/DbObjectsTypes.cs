namespace DotNetDBTools.Deploy.Core
{
    internal enum DbObjectsTypes
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
        UserDefinedType,
        UserDefinedTableType,
    }
}
