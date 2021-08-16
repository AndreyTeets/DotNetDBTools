using DotNetDBTools.Models.SQLite;
using System.Collections.Generic;
using System.Linq;

namespace DotNetDBTools.DeployInteractor.SQLite
{
    internal static class Queries
    {
        public static readonly string GetExistingTables =
$@"select
    {DNDBTSysTables.DNDBTDbObjects.Metadata}
from {DNDBTSysTables.DNDBTDbObjects}
where {DNDBTSysTables.DNDBTDbObjects.Type} = '{SQLiteDbObjectsTypes.Table}'";

        public static string CreateTable(SQLiteTableInfo table)
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
    '{SQLiteDbObjectsTypes.Table}',
    '{table.Name}',
    @{DNDBTSysTables.DNDBTDbObjects.Metadata}
)

create table {table.Name}
(
{string.Join(",\n", tableDefinitions)}
)";

            return query;
        }
    }
}
