using DotNetDBTools.Models.MSSQL;
using System.Collections.Generic;
using System.Linq;

namespace DotNetDBTools.DeployInteractor.MSSQL
{
    internal static class Queries
    {
        public static readonly string GetExistingTables =
$@"select
    {DNDBTSysTables.DNDBTDbObjects.Metadata}
from {DNDBTSysTables.DNDBTDbObjects}
where {DNDBTSysTables.DNDBTDbObjects.Type} = '{MSSQLDbObjectsTypes.Table}'";

        public static string CreateTable(MSSQLTableInfo table)
        {
            List<string> tableDefinitions = new();
            tableDefinitions.AddRange(table.Columns.Select(column => $@"    {column.Name} {column.DataType}"));
            tableDefinitions.AddRange(table.ForeignKeys.Select(fk => $@"    constraint {fk.Name} foreign key ({string.Join(",", fk.ThisColumnNames)}) references {fk.ForeignTableName}({string.Join(",", fk.ForeignColumnNames)})"));

            string query =
$@"insert into {DNDBTSysTables.DNDBTDbObjects}
(
    {DNDBTSysTables.DNDBTDbObjects.ID},
    {DNDBTSysTables.DNDBTDbObjects.Type},
    {DNDBTSysTables.DNDBTDbObjects.Name},
    {DNDBTSysTables.DNDBTDbObjects.Metadata}
)
values
(
    '{table.ID}',
    '{MSSQLDbObjectsTypes.Table}',
    '{table.Name}',
    @{DNDBTSysTables.DNDBTDbObjects.Metadata}
)

create table {table.Name}
(
{string.Join(",\n", tableDefinitions)}
)";

            return query;
        }

        public static string DropTable(MSSQLTableInfo table)
        {
            string query =
$@"DROP TABLE {table.Name};

DELETE FROM {DNDBTSysTables.DNDBTDbObjects}
WHERE {DNDBTSysTables.DNDBTDbObjects.ID} = '{table.ID}';";

            return query;
        }
    }
}
