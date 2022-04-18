using DotNetDBTools.Analysis.Core;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MSSQL;

namespace DotNetDBTools.Analysis.MSSQL;

public class MSSQLDbModelConverter : DbModelConverter<
    MSSQLDatabase,
    MSSQLTable,
    MSSQLView,
    MSSQLColumn>
{
    public MSSQLDbModelConverter() : base(
        DatabaseKind.MSSQL,
        new MSSQLDataTypeConverter(),
        new MSSQLDefaultValueConverter(),
        new MSSQLDbModelPostProcessor())
    {
    }

    protected override void BuildAdditionalColumnProperties(MSSQLColumn column, string tableName)
    {
        column.DefaultConstraintName = column.Default.Code is not null
            ? $"DF_{tableName}_{column.Name}"
            : null;
    }
}
