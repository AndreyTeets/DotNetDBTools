using DotNetDBTools.Analysis.Core;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.PostgreSQL;

namespace DotNetDBTools.Analysis.PostgreSQL;

internal class PostgreSQLDbModelConverter : DbModelConverter<
    PostgreSQLDatabase,
    PostgreSQLTable,
    PostgreSQLView,
    PostgreSQLIndex,
    PostgreSQLTrigger,
    PostgreSQLColumn>
{
    public PostgreSQLDbModelConverter() : base(
        DatabaseKind.PostgreSQL,
        new PostgreSQLDataTypeConverter(),
        new PostgreSQLDefaultValueConverter(),
        new PostgreSQLDependenciesBuilder(),
        new PostgreSQLDbModelPostProcessor())
    {
    }

    protected override void BuildAdditionalColumnProperties(PostgreSQLColumn column, string tableName)
    {
        if (column.Identity)
        {
            column.IdentityGenerationKind = "ALWAYS";
            column.IdentitySequenceOptions = new PostgreSQLSequenceOptions()
            {
                StartWith = 1,
                IncrementBy = 1,
                MinValue = 1,
                MaxValue = int.MaxValue,
                Cache = 1,
                Cycle = false,
            };
        }
    }
}
