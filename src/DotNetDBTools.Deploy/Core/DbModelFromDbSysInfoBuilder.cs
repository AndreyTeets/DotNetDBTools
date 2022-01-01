using System;
using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Deploy.Core.Queries.DBMSSysInfo;
using DotNetDBTools.Deploy.Core.Queries.DNDBTSysInfo;
using DotNetDBTools.Models.Core;
using static DotNetDBTools.Deploy.Core.Queries.DBMSSysInfo.GetCheckConstraintsFromDBMSSysInfoQuery;
using static DotNetDBTools.Deploy.Core.Queries.DBMSSysInfo.GetColumnsFromDBMSSysInfoQuery;
using static DotNetDBTools.Deploy.Core.Queries.DBMSSysInfo.GetForeignKeysFromDBMSSysInfoQuery;
using static DotNetDBTools.Deploy.Core.Queries.DBMSSysInfo.GetPrimaryKeysFromDBMSSysInfoQuery;
using static DotNetDBTools.Deploy.Core.Queries.DBMSSysInfo.GetUniqueConstraintsFromDBMSSysInfoQuery;
using static DotNetDBTools.Deploy.Core.Queries.DBMSSysInfo.GetViewsFromDBMSSysInfoQuery;
using static DotNetDBTools.Deploy.Core.Queries.DNDBTSysInfo.GetAllDbObjectsFromDNDBTSysInfoQuery;

namespace DotNetDBTools.Deploy.Core
{
    internal abstract class DbModelFromDbSysInfoBuilder<
        TDatabase,
        TTable,
        TView,
        TGetColumnsFromDBMSSysInfoQuery,
        TGetPrimaryKeysFromDBMSSysInfoQuery,
        TGetUniqueConstraintsFromDBMSSysInfoQuery,
        TGetCheckConstraintsFromDBMSSysInfoQuery,
        TGetForeignKeysFromDBMSSysInfoQuery,
        TGetViewsFromDBMSSysInfoQuery,
        TGetAllDbObjectsFromDNDBTSysInfoQuery>
        : IDbModelFromDbSysInfoBuilder
        where TDatabase : Database, new()
        where TTable : Table, new()
        where TView : View, new()
        where TGetColumnsFromDBMSSysInfoQuery : GetColumnsFromDBMSSysInfoQuery, new()
        where TGetPrimaryKeysFromDBMSSysInfoQuery : GetPrimaryKeysFromDBMSSysInfoQuery, new()
        where TGetUniqueConstraintsFromDBMSSysInfoQuery : GetUniqueConstraintsFromDBMSSysInfoQuery, new()
        where TGetCheckConstraintsFromDBMSSysInfoQuery : GetCheckConstraintsFromDBMSSysInfoQuery, new()
        where TGetForeignKeysFromDBMSSysInfoQuery : GetForeignKeysFromDBMSSysInfoQuery, new()
        where TGetViewsFromDBMSSysInfoQuery : GetViewsFromDBMSSysInfoQuery, new()
        where TGetAllDbObjectsFromDNDBTSysInfoQuery : GetAllDbObjectsFromDNDBTSysInfoQuery, new()
    {
        protected readonly IQueryExecutor QueryExecutor;

        protected DbModelFromDbSysInfoBuilder(IQueryExecutor queryExecutor)
        {
            QueryExecutor = queryExecutor;
        }

        public Database GetDatabaseModelFromDNDBTSysInfo()
        {
            Database database = GenerateDatabaseModelFromDBMSSysInfo();
            ReplaceDbModelObjectsIDsAndCodeWithDNDBTSysInfo(database);
            return database;
        }

        public Database GenerateDatabaseModelFromDBMSSysInfo()
        {
            TDatabase database = new();
            database.Tables = BuildTables();
            database.Views = BuildViews();
            BuildAdditionalDbObjects(database);
            return database;
        }
        protected virtual void BuildAdditionalDbObjects(Database database) { }

        private void ReplaceDbModelObjectsIDsAndCodeWithDNDBTSysInfo(Database database)
        {
            TGetAllDbObjectsFromDNDBTSysInfoQuery query = new();
            IEnumerable<DNDBTDbObjectRecord> dbObjectRecords = query.Loader.GetRecords(QueryExecutor, query);
            Dictionary<string, DNDBTInfo> dbObjectIDsMap = new();
            foreach (DNDBTDbObjectRecord dbObjRec in dbObjectRecords)
            {
                DNDBTInfo dndbtInfo = new() { ID = dbObjRec.GetID(), Code = dbObjRec.Code };
                dbObjectIDsMap.Add($"{dbObjRec.Type}_{dbObjRec.Name}_{dbObjRec.GetParentID()}", dndbtInfo);
            }

            foreach (Table table in database.Tables)
            {
                table.ID = dbObjectIDsMap[$"{DbObjectsTypes.Table}_{table.Name}_{null}"].ID;
                foreach (Column column in table.Columns)
                {
                    DNDBTInfo dndbtInfo = dbObjectIDsMap[$"{DbObjectsTypes.Column}_{column.Name}_{table.ID}"];
                    column.ID = dndbtInfo.ID;
                    if (column.Default is CodePiece codePiece)
                        codePiece.Code = dndbtInfo.Code;
                }

                if (table.PrimaryKey is not null)
                    table.PrimaryKey.ID = dbObjectIDsMap[$"{DbObjectsTypes.PrimaryKey}_{table.PrimaryKey.Name}_{table.ID}"].ID;
                foreach (UniqueConstraint uc in table.UniqueConstraints)
                    uc.ID = dbObjectIDsMap[$"{DbObjectsTypes.UniqueConstraint}_{uc.Name}_{table.ID}"].ID;
                foreach (CheckConstraint ck in table.CheckConstraints)
                {
                    DNDBTInfo dndbtInfoCK = dbObjectIDsMap[$"{DbObjectsTypes.CheckConstraint}_{ck.Name}_{table.ID}"];
                    ck.ID = dndbtInfoCK.ID;
                    ck.CodePiece.Code = dndbtInfoCK.Code;
                }
                foreach (ForeignKey fk in table.ForeignKeys)
                    fk.ID = dbObjectIDsMap[$"{DbObjectsTypes.ForeignKey}_{fk.Name}_{table.ID}"].ID;
            }

            foreach (View view in database.Views)
            {
                DNDBTInfo dndbtInfo = dbObjectIDsMap[$"{DbObjectsTypes.View}_{view.Name}_{null}"];
                view.ID = dndbtInfo.ID;
                view.CodePiece.Code = dndbtInfo.Code;
            }

            ReplaceAdditionalDbModelObjectsIDsAndCodeWithDNDBTSysInfo(database, dbObjectIDsMap);
        }
        protected virtual void ReplaceAdditionalDbModelObjectsIDsAndCodeWithDNDBTSysInfo(Database database, Dictionary<string, DNDBTInfo> dbObjectIDsMap) { }

        private IEnumerable<Table> BuildTables()
        {
            Dictionary<string, Table> tables = BuildTablesListWithColumns();
            BuildTablesPrimaryKeys(tables);
            BuildTablesUniqueConstraints(tables);
            BuildTablesCheckConstraints(tables);
            BuildTablesForeignKeys(tables);
            BuildAdditionalTablesAttributes(tables);
            return tables.Select(x => x.Value);
        }
        protected virtual void BuildAdditionalTablesAttributes(Dictionary<string, Table> tables) { }

        private IEnumerable<View> BuildViews()
        {
            TGetViewsFromDBMSSysInfoQuery query = new();
            IEnumerable<ViewRecord> viewRecords = QueryExecutor.Query<ViewRecord>(query);
            List<View> views = new();
            foreach (ViewRecord viewRecord in viewRecords)
            {
                TView view = new()
                {
                    ID = Guid.NewGuid(),
                    Name = viewRecord.ViewName,
                    CodePiece = new CodePiece { Code = viewRecord.ViewCode },
                };
                views.Add(view);
            }
            return views;
        }

        private Dictionary<string, Table> BuildTablesListWithColumns()
        {
            TGetColumnsFromDBMSSysInfoQuery query = new();
            IEnumerable<ColumnRecord> columnRecords = query.Loader.GetRecords(QueryExecutor, query);
            Dictionary<string, Table> tables = new();
            foreach (ColumnRecord columnRecord in columnRecords)
            {
                if (!tables.ContainsKey(columnRecord.TableName))
                {
                    TTable table = new()
                    {
                        ID = Guid.NewGuid(),
                        Name = columnRecord.TableName,
                        Columns = new List<Column>(),
                        UniqueConstraints = new List<UniqueConstraint>(),
                        CheckConstraints = new List<CheckConstraint>(),
                        Indexes = new List<Index>(),
                        Triggers = new List<Trigger>(),
                        ForeignKeys = new List<ForeignKey>(),
                    };
                    tables.Add(columnRecord.TableName, table);
                }
                Column column = query.Mapper.MapToColumnModel(columnRecord);
                ((List<Column>)tables[columnRecord.TableName].Columns).Add(column);
            }
            return tables;
        }

        private void BuildTablesPrimaryKeys(Dictionary<string, Table> tables)
        {
            TGetPrimaryKeysFromDBMSSysInfoQuery query = new();
            IEnumerable<PrimaryKeyRecord> primaryKeyRecords = QueryExecutor.Query<PrimaryKeyRecord>(query);
            Dictionary<string, SortedDictionary<int, string>> columnNames = new();
            foreach (PrimaryKeyRecord pkr in primaryKeyRecords)
            {
                if (!columnNames.ContainsKey(pkr.ConstraintName))
                    columnNames.Add(pkr.ConstraintName, new SortedDictionary<int, string>());

                columnNames[pkr.ConstraintName].Add(
                    pkr.ColumnPosition, pkr.ColumnName);

                tables[pkr.TableName].PrimaryKey = query.Mapper.MapExceptColumnsToPrimaryKeyModel(pkr);
            }

            foreach (Table table in tables.Values)
            {
                if (table.PrimaryKey is null)
                    continue;
                table.PrimaryKey.Columns = columnNames[table.PrimaryKey.Name].Select(x => x.Value).ToList();
            }
        }

        private void BuildTablesUniqueConstraints(Dictionary<string, Table> tables)
        {
            TGetUniqueConstraintsFromDBMSSysInfoQuery query = new();
            IEnumerable<UniqueConstraintRecord> uniqueConstraintRecords = QueryExecutor.Query<UniqueConstraintRecord>(query);
            Dictionary<string, SortedDictionary<int, string>> columnNames = new();
            foreach (UniqueConstraintRecord ucr in uniqueConstraintRecords)
            {
                if (!columnNames.ContainsKey(ucr.ConstraintName))
                    columnNames.Add(ucr.ConstraintName, new SortedDictionary<int, string>());

                columnNames[ucr.ConstraintName].Add(
                    ucr.ColumnPosition, ucr.ColumnName);

                UniqueConstraint uc = query.Mapper.MapExceptColumnsToUniqueConstraintModel(ucr);
                ((List<UniqueConstraint>)tables[ucr.TableName].UniqueConstraints).Add(uc);
            }

            foreach (Table table in tables.Values)
            {
                foreach (UniqueConstraint uc in table.UniqueConstraints)
                {
                    uc.Columns = columnNames[uc.Name].Select(x => x.Value).ToList();
                }
            }
        }

        private void BuildTablesCheckConstraints(Dictionary<string, Table> tables)
        {
            TGetCheckConstraintsFromDBMSSysInfoQuery query = new();
            IEnumerable<CheckConstraintRecord> checkConstraintRecords = QueryExecutor.Query<CheckConstraintRecord>(query);
            foreach (CheckConstraintRecord ckr in checkConstraintRecords)
            {
                CheckConstraint ck = query.Mapper.MapToCheckConstraintModel(ckr);
                ((List<CheckConstraint>)tables[ckr.TableName].CheckConstraints).Add(ck);
            }
        }

        private void BuildTablesForeignKeys(Dictionary<string, Table> tables)
        {
            TGetForeignKeysFromDBMSSysInfoQuery query = new();
            IEnumerable<ForeignKeyRecord> foreignKeyRecords = QueryExecutor.Query<ForeignKeyRecord>(query);
            Dictionary<string, SortedDictionary<int, string>> thisColumnNames = new();
            Dictionary<string, SortedDictionary<int, string>> referencedColumnNames = new();
            Dictionary<string, HashSet<string>> addedForeignKeysForTable = new();
            foreach (ForeignKeyRecord fkr in foreignKeyRecords)
            {
                if (!addedForeignKeysForTable.ContainsKey(fkr.ThisTableName))
                    addedForeignKeysForTable.Add(fkr.ThisTableName, new HashSet<string>());

                if (!thisColumnNames.ContainsKey(fkr.ForeignKeyName))
                    thisColumnNames.Add(fkr.ForeignKeyName, new SortedDictionary<int, string>());
                if (!referencedColumnNames.ContainsKey(fkr.ForeignKeyName))
                    referencedColumnNames.Add(fkr.ForeignKeyName, new SortedDictionary<int, string>());

                thisColumnNames[fkr.ForeignKeyName].Add(
                    fkr.ThisColumnPosition, fkr.ThisColumnName);
                referencedColumnNames[fkr.ForeignKeyName].Add(
                    fkr.ReferencedColumnPosition, fkr.ReferencedColumnName);

                if (!addedForeignKeysForTable[fkr.ThisTableName].Contains(fkr.ForeignKeyName))
                {
                    ForeignKey fk = query.Mapper.MapExceptColumnsToForeignKeyModel(fkr);
                    ((List<ForeignKey>)tables[fkr.ThisTableName].ForeignKeys).Add(fk);
                    addedForeignKeysForTable[fkr.ThisTableName].Add(fkr.ForeignKeyName);
                }
            }

            foreach (Table table in tables.Values)
            {
                foreach (ForeignKey fk in table.ForeignKeys)
                {
                    fk.ThisColumnNames = thisColumnNames[fk.Name].Select(x => x.Value).ToList();
                    fk.ReferencedTableColumnNames = referencedColumnNames[fk.Name].Select(x => x.Value).ToList();
                }
            }
        }
    }
}
