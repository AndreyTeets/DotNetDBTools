using System;
using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.SQLite.Queries.DBMSSysInfo;
using DotNetDBTools.Deploy.SQLite.Queries.DNDBTSysInfo;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.SQLite;
using static DotNetDBTools.Deploy.SQLite.Queries.DBMSSysInfo.SQLiteGetTablesDefinitionsFromDBMSSysInfoQuery;

namespace DotNetDBTools.Deploy.SQLite
{
    internal class SQLiteDbModelFromDbSysInfoBuilder : DbModelFromDbSysInfoBuilder<
        SQLiteDatabase,
        SQLiteTable,
        SQLiteGetColumnsFromDBMSSysInfoQuery,
        SQLiteGetPrimaryKeysFromDBMSSysInfoQuery,
        SQLiteGetUniqueConstraintsFromDBMSSysInfoQuery,
        SQLiteGetCheckConstraintsFromDBMSSysInfoQuery,
        SQLiteGetForeignKeysFromDBMSSysInfoQuery,
        SQLiteGetAllDbObjectsFromDNDBTSysInfoQuery>
    {
        public SQLiteDbModelFromDbSysInfoBuilder(IQueryExecutor queryExecutor)
            : base(queryExecutor) { }

        protected override void BuildAdditionalTablesAttributes(Dictionary<string, Table> tables)
        {
            SQLiteGetTablesDefinitionsFromDBMSSysInfoQuery query = new();
            IEnumerable<TableRecord> tableRecords = QueryExecutor.Query<TableRecord>(query);
            foreach (TableRecord tableRecord in tableRecords)
            {
                IEnumerable<string> definitionStatements = SQLiteTableDefinitionParser.ParseToDefinitionStatements(tableRecord.TableDefinition);
                Table table = tables[tableRecord.TableName];
                BuildTableCheckConstraints(table, definitionStatements);
                BuildTableConstraintNames(table, definitionStatements);
                ProcessTableIdentityColumnCandidateIfExist(table, definitionStatements);
            }
        }

        private static void BuildTableCheckConstraints(Table table, IEnumerable<string> definitionStatements)
        {
            foreach ((string ckName, string ckCode) in SQLiteTableDefinitionParser.GetCheckConstraints(definitionStatements))
            {
                CheckConstraint ck = new()
                {
                    ID = Guid.NewGuid(),
                    Name = ckName,
                    CodePiece = new CodePiece { Code = ckCode },
                };
                ((List<CheckConstraint>)table.CheckConstraints).Add(ck);
            }
        }

        private void BuildTableConstraintNames(Table table, IEnumerable<string> definitionStatements)
        {
            foreach (UniqueConstraint uc in table.UniqueConstraints)
                uc.Name = SQLiteTableDefinitionParser.GetUniqueConstraintName(definitionStatements, uc.Columns) ?? uc.Name;
            foreach (ForeignKey fk in table.ForeignKeys)
                fk.Name = SQLiteTableDefinitionParser.GetForeignKeyConstraintName(definitionStatements, fk.ThisColumnNames) ?? fk.Name;
        }

        private void ProcessTableIdentityColumnCandidateIfExist(Table table, IEnumerable<string> definitionStatements)
        {
            Column column = table.Columns.FirstOrDefault(c => c.Identity == true);
            if (column is not null)
            {
                bool hasAutoincrementKeyWord = definitionStatements
                    .Any(x => x.IndexOf("AUTOINCREMENT", StringComparison.OrdinalIgnoreCase) > -1);
                if (!hasAutoincrementKeyWord)
                    column.Identity = false;
            }
        }
    }
}
