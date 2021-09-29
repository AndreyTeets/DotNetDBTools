using System;
using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.Core.ModelBuilders
{
    internal static class ForeignKeysBuilder
    {
        public static void BuildTablesForeignKeys(
            Dictionary<string, TableInfo> tables,
            IEnumerable<ForeignKeyRecord> foreignKeyRecords,
            Func<string, string> mapUpdateActionName)
        {
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
                    ForeignKeyInfo foreignKeyInfo = MapExceptColumnsToForeignKeyInfo(fkr, mapUpdateActionName);
                    ((List<ForeignKeyInfo>)tables[fkr.ThisTableName].ForeignKeys).Add(foreignKeyInfo);
                    addedForeignKeysForTable[fkr.ThisTableName].Add(fkr.ForeignKeyName);
                }
            }

            foreach (TableInfo table in tables.Values)
            {
                foreach (ForeignKeyInfo foreignKeyInfo in table.ForeignKeys)
                {
                    foreignKeyInfo.ThisColumnNames = thisColumnNames[foreignKeyInfo.Name].Select(x => x.Value).ToList();
                    foreignKeyInfo.ReferencedTableColumnNames = referencedColumnNames[foreignKeyInfo.Name].Select(x => x.Value).ToList();
                }
            }
        }

        private static ForeignKeyInfo MapExceptColumnsToForeignKeyInfo(ForeignKeyRecord fkr, Func<string, string> mapUpdateActionName)
        {
            return new ForeignKeyInfo()
            {
                ID = Guid.NewGuid(),
                Name = fkr.ForeignKeyName,
                ReferencedTableName = fkr.ReferencedTableName,
                OnUpdate = mapUpdateActionName(fkr.OnUpdate),
                OnDelete = mapUpdateActionName(fkr.OnDelete),
            };
        }

        internal class ForeignKeyRecord
        {
            public string ThisTableName { get; set; }
            public string ForeignKeyName { get; set; }
            public string ThisColumnName { get; set; }
            public int ThisColumnPosition { get; set; }
            public string ReferencedTableName { get; set; }
            public string ReferencedColumnName { get; set; }
            public int ReferencedColumnPosition { get; set; }
            public string OnUpdate { get; set; }
            public string OnDelete { get; set; }
        }
    }
}
