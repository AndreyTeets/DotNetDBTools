using DotNetDBTools.Deploy.Common.Queries.DDL;
using DotNetDBTools.Models.Core;
using static DotNetDBTools.Deploy.MSSQL.MSSQLQueriesHelper;

namespace DotNetDBTools.Deploy.MSSQL.Queries.DDL
{
    internal class MSSQLCreateForeignKeyQuery : CreateForeignKeyQuery
    {
        public MSSQLCreateForeignKeyQuery(ForeignKey fk, string tableName)
            : base(fk, tableName) { }

        protected override string GetSql(ForeignKey fk, string tableName)
        {
            string query =
$@"ALTER TABLE {tableName} ADD CONSTRAINT {fk.Name} FOREIGN KEY ({string.Join(", ", fk.ThisColumnNames)})
    REFERENCES {fk.ReferencedTableName} ({string.Join(", ", fk.ReferencedTableColumnNames)})
    ON UPDATE {MapActionName(fk.OnUpdate)} ON DELETE {MapActionName(fk.OnDelete)};";

            return query;
        }
    }
}
