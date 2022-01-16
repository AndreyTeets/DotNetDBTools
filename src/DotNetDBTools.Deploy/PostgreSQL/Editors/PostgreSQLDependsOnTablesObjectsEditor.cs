using System;
using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Analysis.Core;
using DotNetDBTools.Deploy.Common.Editors;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Queries;
using DotNetDBTools.Deploy.PostgreSQL.Queries.DNDBTSysInfo;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.PostgreSQL;

namespace DotNetDBTools.Deploy.PostgreSQL.Editors
{
    internal class PostgreSQLDependsOnTablesObjectsEditor
    {
        private readonly IIndexEditor _indexEditor;
        private readonly ITriggerEditor _triggerEditor;
        private readonly IForeignKeyEditor _foreignKeyEditor;

        protected readonly IQueryExecutor QueryExecutor;

        public PostgreSQLDependsOnTablesObjectsEditor(IQueryExecutor queryExecutor)
        {
            _indexEditor = new PostgreSQLIndexEditor(queryExecutor);
            _triggerEditor = new PostgreSQLTriggerEditor(queryExecutor);
            _foreignKeyEditor = new PostgreSQLForeignKeyEditor(queryExecutor);
            QueryExecutor = queryExecutor;
        }

        public void CreateObjectsThatDependOnTables(PostgreSQLDatabaseDiff dbDiff)
        {
            _indexEditor.CreateIndexes(dbDiff);
            _foreignKeyEditor.CreateForeignKeys(dbDiff);
            foreach (DBObject dbObject in GetOrderedByDependenciesObjectsToCreate(dbDiff))
                CreateDbObject(dbObject);
            _triggerEditor.CreateTriggers(dbDiff);
        }

        public void DropObjectsThatDependOnTables(PostgreSQLDatabaseDiff dbDiff)
        {
            _triggerEditor.DropTriggers(dbDiff);
            foreach (DBObject dbObject in GetOrderedByDependenciesObjectsToDrop(dbDiff))
                DropDbObject(dbObject);
            _foreignKeyEditor.DropForeignKeys(dbDiff);
            _indexEditor.DropIndexes(dbDiff);
        }

        private static IEnumerable<DBObject> GetOrderedByDependenciesObjectsToCreate(PostgreSQLDatabaseDiff dbDiff)
        {
            return dbDiff.FunctionsToCreate.Where(x => !x.IsSimple).Select(x => (DBObject)x)
                .Concat(dbDiff.ViewsToCreate.Select(x => (DBObject)x))
                .Concat(dbDiff.ProceduresToCreate.Select(x => (DBObject)x))
                .OrderByDependenciesFirst();
        }

        private static IEnumerable<DBObject> GetOrderedByDependenciesObjectsToDrop(PostgreSQLDatabaseDiff dbDiff)
        {
            return dbDiff.FunctionsToDrop.Where(x => !x.IsSimple).Select(x => (DBObject)x)
                .Concat(dbDiff.ViewsToDrop.Select(x => (DBObject)x))
                .Concat(dbDiff.ProceduresToDrop.Select(x => (DBObject)x))
                .OrderByDependenciesLast();
        }

        private void CreateDbObject(DBObject dbObject)
        {
            if (dbObject is PostgreSQLView view)
                CreateView(view);
            else if (dbObject is PostgreSQLFunction func)
                CreateFunction(func);
            else if (dbObject is PostgreSQLProcedure proc)
                CreateProcedure(proc);
            else
                throw new InvalidOperationException($"Invalid dbObject type to create {dbObject.GetType()}");
        }

        private void DropDbObject(DBObject dbObject)
        {
            if (dbObject is PostgreSQLView view)
                DropView(view);
            else if (dbObject is PostgreSQLFunction func)
                DropFunction(func);
            else if (dbObject is PostgreSQLProcedure proc)
                DropProcedure(proc);
            else
                throw new InvalidOperationException($"Invalid dbObject type to drop {dbObject.GetType()}");
        }

        private void CreateView(PostgreSQLView view)
        {
            QueryExecutor.Execute(new GenericQuery($"{view.GetCode()}"));
            QueryExecutor.Execute(new PostgreSQLInsertDNDBTSysInfoQuery(view.ID, null, DbObjectsTypes.View, view.Name, view.GetCode()));
        }

        private void DropView(PostgreSQLView view)
        {
            QueryExecutor.Execute(new GenericQuery($@"DROP VIEW ""{view.Name}"";"));
            QueryExecutor.Execute(new PostgreSQLDeleteDNDBTSysInfoQuery(view.ID));
        }

        private void CreateFunction(PostgreSQLFunction func)
        {
            QueryExecutor.Execute(new GenericQuery($"{func.GetCode()}"));
            QueryExecutor.Execute(new PostgreSQLInsertDNDBTSysInfoQuery(func.ID, null, DbObjectsTypes.Function, func.Name, func.GetCode()));
        }

        private void DropFunction(PostgreSQLFunction func)
        {
            QueryExecutor.Execute(new GenericQuery($@"DROP FUNCTION ""{func.Name}"";"));
            QueryExecutor.Execute(new PostgreSQLDeleteDNDBTSysInfoQuery(func.ID));
        }

        private void CreateProcedure(PostgreSQLProcedure proc)
        {
            QueryExecutor.Execute(new GenericQuery($"{proc.GetCode()}"));
            QueryExecutor.Execute(new PostgreSQLInsertDNDBTSysInfoQuery(proc.ID, null, DbObjectsTypes.Procedure, proc.Name, proc.GetCode()));
        }

        private void DropProcedure(PostgreSQLProcedure proc)
        {
            QueryExecutor.Execute(new GenericQuery($@"DROP PROCEDURE ""{proc.Name}"";"));
            QueryExecutor.Execute(new PostgreSQLDeleteDNDBTSysInfoQuery(proc.ID));
        }
    }
}
