using System;
using System.Collections.Generic;
using DotNetDBTools.Deploy.Common.Queries.DDL;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Queries.DNDBTSysInfo;
using DotNetDBTools.Models.Core;
using static DotNetDBTools.Deploy.Core.InstanceCreator;

namespace DotNetDBTools.Deploy.Common.Editors
{
    internal abstract class IndexEditor<
        TInsertDNDBTSysInfoQuery,
        TDeleteDNDBTSysInfoQuery,
        TCreateIndexQuery,
        TDropIndexQuery>
        : IIndexEditor
        where TInsertDNDBTSysInfoQuery : InsertDNDBTSysInfoQuery
        where TDeleteDNDBTSysInfoQuery : DeleteDNDBTSysInfoQuery
        where TCreateIndexQuery : CreateIndexQuery
        where TDropIndexQuery : DropIndexQuery
    {
        protected readonly IQueryExecutor QueryExecutor;

        protected IndexEditor(IQueryExecutor queryExecutor)
        {
            QueryExecutor = queryExecutor;
        }

        public void CreateIndexes(DatabaseDiff dbDiff)
        {
            Dictionary<Guid, Table> indexToTableMap = CreateIndexToTableMap(dbDiff.NewDatabase.Tables);
            foreach (Index index in dbDiff.IndexesToCreate)
                CreateIndex(index, indexToTableMap[index.ID]);
        }

        public void DropIndexes(DatabaseDiff dbDiff)
        {
            Dictionary<Guid, Table> indexToTableMap = CreateIndexToTableMap(dbDiff.OldDatabase.Tables);
            foreach (Index index in dbDiff.IndexesToDrop)
                DropIndex(index, indexToTableMap[index.ID]);
        }

        private void CreateIndex(Index index, Table table)
        {
            QueryExecutor.Execute(Create<TCreateIndexQuery>(index, table));
            QueryExecutor.Execute(Create<TInsertDNDBTSysInfoQuery>(index.ID, table.ID, DbObjectType.Index, index.Name));
        }

        private void DropIndex(Index index, Table table)
        {
            QueryExecutor.Execute(Create<TDropIndexQuery>(index, table));
            QueryExecutor.Execute(Create<TDeleteDNDBTSysInfoQuery>(index.ID));
        }

        private static Dictionary<Guid, Table> CreateIndexToTableMap(IEnumerable<Table> tables)
        {
            Dictionary<Guid, Table> indexToTableMap = new();
            foreach (Table table in tables)
            {
                foreach (Index index in table.Indexes)
                    indexToTableMap.Add(index.ID, table);
            }
            return indexToTableMap;
        }
    }
}
