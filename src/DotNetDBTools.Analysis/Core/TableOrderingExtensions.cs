using System;
using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Analysis.Core
{
    public static class TableOrderingExtensions
    {
        public static IEnumerable<TableInfo> PutReferencedLast(this IEnumerable<TableInfo> tables)
        {
            if (!tables.Any())
                return tables;
            List<TableInfo> orderedTables = new();
            IEnumerable<IEnumerable<TableInfo>> orderedGroups = GetOrderedGroups(tables);
            foreach (IEnumerable<TableInfo> group in orderedGroups)
                orderedTables.AddRange(group);
            return orderedTables;
        }

        public static IEnumerable<TableInfo> PutReferencedFirst(this IEnumerable<TableInfo> tables)
        {
            if (!tables.Any())
                return tables;
            List<TableInfo> orderedTables = new();
            IEnumerable<IEnumerable<TableInfo>> orderedGroups = GetOrderedGroups(tables);
            foreach (IEnumerable<TableInfo> group in orderedGroups.Reverse())
                orderedTables.AddRange(group);
            return orderedTables;
        }

        private static IEnumerable<IEnumerable<TableInfo>> GetOrderedGroups(IEnumerable<TableInfo> tables)
        {
            List<IEnumerable<TableInfo>> orderedGroups = new();
            HashSet<TableInfo> uprocessedTables = new(tables);
            HashSet<string> processedTables = new();
            Dictionary<string, HashSet<string>> referencedByMap = CreateReferencedByMap(tables);
            AddTablesUnreferencedByAnyOtherUnprocessedTable(orderedGroups, uprocessedTables, processedTables, referencedByMap);
            return orderedGroups;
        }

        private static void AddTablesUnreferencedByAnyOtherUnprocessedTable(
            List<IEnumerable<TableInfo>> orderedGroups,
            HashSet<TableInfo> uprocessedTables,
            HashSet<string> processedTables,
            Dictionary<string, HashSet<string>> referencedByMap)
        {
            IEnumerable<TableInfo> tablesUnreferencedByAnyOtherUnprocessedTable = uprocessedTables.Where(table =>
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

            IEnumerable<TableInfo> newProcessedGroup = tablesUnreferencedByAnyOtherUnprocessedTable
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
            IEnumerable<TableInfo> tables)
        {
            Dictionary<string, HashSet<string>> referencedByMap = new();
            foreach (TableInfo table in tables)
            {
                IEnumerable<string> referencedTablesNames = table.ForeignKeys.Select(x => x.ReferencedTableName);
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
