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
    Column>
{
    public PostgreSQLDbModelConverter() : base(
        DatabaseKind.PostgreSQL,
        new PostgreSQLDataTypeConverter(),
        new PostgreSQLDefaultValueConverter(),
        new PostgreSQLDependenciesBuilder(),
        new PostgreSQLDbModelPostProcessor())
    {
    }
}
