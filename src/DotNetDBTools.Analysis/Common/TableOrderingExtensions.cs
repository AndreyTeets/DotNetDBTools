using System;
using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Models.Common;

namespace DotNetDBTools.Analysis.Common
{
    public static class TableOrderingExtensions
    {
        public static IEnumerable<ITableInfo<IColumnInfo>> PutReferencedLast(this IEnumerable<ITableInfo<IColumnInfo>> tables)
        {
            if (!tables.Any())
                return tables;
            List<ITableInfo<IColumnInfo>> orderedTables = new();
            IEnumerable<IEnumerable<ITableInfo<IColumnInfo>>> orderedGroups = GetOrderedGroups(tables);
            foreach (IEnumerable<ITableInfo<IColumnInfo>> group in orderedGroups)
                orderedTables.AddRange(group);
            return orderedTables;
        }

        public static IEnumerable<ITableInfo<IColumnInfo>> PutReferencedFirst(this IEnumerable<ITableInfo<IColumnInfo>> tables)
        {
            if (!tables.Any())
                return tables;
            List<ITableInfo<IColumnInfo>> orderedTables = new();
            IEnumerable<IEnumerable<ITableInfo<IColumnInfo>>> orderedGroups = GetOrderedGroups(tables);
            foreach (IEnumerable<ITableInfo<IColumnInfo>> group in orderedGroups.Reverse())
                orderedTables.AddRange(group);
            return orderedTables;
        }

        private static IEnumerable<IEnumerable<ITableInfo<IColumnInfo>>> GetOrderedGroups(IEnumerable<ITableInfo<IColumnInfo>> tables)
        {
            List<IEnumerable<ITableInfo<IColumnInfo>>> orderedGroups = new();
            HashSet<ITableInfo<IColumnInfo>> uprocessedTables = new(tables);
            HashSet<string> processedTables = new();
            Dictionary<string, HashSet<string>> referencedByMap = CreateReferencedByMap(tables);
            AddTablesUnreferencedByAnyOtherUnprocessedTable(orderedGroups, uprocessedTables, processedTables, referencedByMap);
            return orderedGroups;
        }

        private static void AddTablesUnreferencedByAnyOtherUnprocessedTable(
            List<IEnumerable<ITableInfo<IColumnInfo>>> orderedGroups,
            HashSet<ITableInfo<IColumnInfo>> uprocessedTables,
            HashSet<string> processedTables,
            Dictionary<string, HashSet<string>> referencedByMap)
        {
            IEnumerable<ITableInfo<IColumnInfo>> tablesUnreferencedByAnyOtherUnprocessedTable = uprocessedTables.Where(table =>
            {
                bool tableIsReferencedByAnotherUnprocessedTable;
                if (referencedByMap.TryGetValue(table.Name, out HashSet<string> referencingTables))
                {
                    IEnumerable<string> uprocessedReferencingTables = referencingTables.Except(processedTables);
                    tableIsReferencedByAnotherUnprocessedTable = uprocessedReferencingTables.Any();
                }
                else
                {
                    tableIsReferencedByAnotherUnprocessedTable = false;
                }
                return !tableIsReferencedByAnotherUnprocessedTable;
            });

            IEnumerable<ITableInfo<IColumnInfo>> newProcessedGroup = tablesUnreferencedByAnyOtherUnprocessedTable
                .OrderBy(table => table.Name, StringComparer.Ordinal)
                .ToList();

            if (!newProcessedGroup.Any())
                throw new Exception("Invalid table references graph, probably with cyclic dependency");

            orderedGroups.Add(newProcessedGroup);
            processedTables.UnionWith(newProcessedGroup.Select(table => table.Name));
            uprocessedTables.ExceptWith(newProcessedGroup);

            if (!uprocessedTables.Any())
                return;

            AddTablesUnreferencedByAnyOtherUnprocessedTable(orderedGroups, uprocessedTables, processedTables, referencedByMap);
        }

        private static Dictionary<string, HashSet<string>> CreateReferencedByMap(
            IEnumerable<ITableInfo<IColumnInfo>> tables)
        {
            Dictionary<string, HashSet<string>> referencedByMap = new();
            foreach (ITableInfo<IColumnInfo> table in tables)
            {
                IEnumerable<string> referencedTablesNames = table.ForeignKeys.Select(x => x.ForeignTableName);
                foreach (string referencedTableName in referencedTablesNames)
                {
                    if (referencedByMap.ContainsKey(referencedTableName))
                        referencedByMap[referencedTableName].Add(table.Name);
                    else
                        referencedByMap.Add(referencedTableName, new HashSet<string>() { table.Name });
                }
            }
            return referencedByMap;
        }
    }
}
