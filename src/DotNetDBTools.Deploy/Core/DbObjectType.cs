namespace DotNetDBTools.Deploy.Core;

internal enum DbObjectType
{
    Sequence,
    UserDefinedType,
    Table,
    Column,
    PrimaryKey,
    UniqueConstraint,
    ForeignKey,
    CheckConstraint,
    Index,
    Trigger,
    View,
    Function,
    Procedure,
    UserDefinedTableType,
}
