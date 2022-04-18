using DotNetDBTools.Analysis.Core;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.PostgreSQL;

namespace DotNetDBTools.Analysis.PostgreSQL;

public class PostgreSQLDbModelConverter : DbModelConverter<
    PostgreSQLDatabase,
    PostgreSQLTable,
    PostgreSQLView,
    Column>
{
    public PostgreSQLDbModelConverter() : base(
        DatabaseKind.PostgreSQL,
        new PostgreSQLDataTypeConverter(),
        new PostgreSQLDefaultValueConverter(),
        new PostgreSQLDbModelPostProcessor())
    {
    }
}
