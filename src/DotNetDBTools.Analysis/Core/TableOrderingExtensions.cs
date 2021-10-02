using System;
using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Analysis.Core
{
    public static class TableOrderingExtensions
    {
        public static IEnumerable<Table> PutReferencedLast(this IEnumerable<Table> tables)
        {
            if (!tables.Any())
                return tables;
            List<Table> orderedTables = new();
            IEnumerable<IEnumerable<Table>> orderedGroups = GetOrderedGroups(tables);
            foreach (IEnumerable<Table> group in orderedGroups)
                orderedTables.AddRange(group);
            return orderedTables;
        }

        public static IEnumerable<Table> PutReferencedFirst(this IEnumerable<Table> tables)
        {
            if (!tables.Any())
                return tables;
            List<Table> orderedTables = new();
            IEnumerable<IEnumerable<Table>> orderedGroups = GetOrderedGroups(tables);
            foreach (IEnumerable<Table> group in orderedGroups.Reverse())
                orderedTables.AddRange(group);
            return orderedTables;
        }

        private static IEnumerable<IEnumerable<Table>> GetOrderedGroups(IEnumerable<Table> tables)
        {
            List<IEnumerable<Table>> orderedGroups = new();
            HashSet<Table> uprocessedTables = new(tables);
            HashSet<string> processedTables = new();
            Dictionary<string, HashSet<string>> referencedByMap = CreateReferencedByMap(tables);
            AddTablesUnreferencedByAnyOtherUnprocessedTable(orderedGroups, uprocessedTables, processedTables, referencedByMap);
            return orderedGroups;
        }

        private static void AddTablesUnreferencedByAnyOtherUnprocessedTable(
            List<IEnumerable<Table>> orderedGroups,
            HashSet<Table> uprocessedTables,
            HashSet<string> processedTables,
            Dictionary<string, HashSet<string>> referencedByMap)
        {
            IEnumerable<Table> tablesUnreferencedByAnyOtherUnprocessedTable = uprocessedTables.Where(table =>
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

            IEnumerable<Table> newProcessedGroup = tablesUnreferencedByAnyOtherUnprocessedTable
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
            IEnumerable<Table> tables)
        {
            Dictionary<string, HashSet<string>> referencedByMap = new();
            foreach (Table table in tables)
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
