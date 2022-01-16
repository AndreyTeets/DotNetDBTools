using System;
using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Analysis.Core
{
    public static class OrderingExtensions
    {
        public static List<TDbObject> OrderByName<TDbObject>(this IEnumerable<TDbObject> dbObjects)
            where TDbObject : DBObject
        {
            return dbObjects.OrderBy(x => x.Name, StringComparer.Ordinal).ToList();
        }

        public static IEnumerable<DBObject> OrderByDependenciesLast(this IEnumerable<DBObject> dbObjects)
        {
            if (!dbObjects.Any())
                return dbObjects;
            List<DBObject> orderedDbObjects = new();
            IEnumerable<IEnumerable<DBObject>> orderedGroups = GetOrderedGroups(dbObjects);
            foreach (IEnumerable<DBObject> group in orderedGroups)
                orderedDbObjects.AddRange(group);
            return orderedDbObjects;
        }

        public static IEnumerable<DBObject> OrderByDependenciesFirst(this IEnumerable<DBObject> dbObjects)
        {
            if (!dbObjects.Any())
                return dbObjects;
            List<DBObject> orderedDbObjects = new();
            IEnumerable<IEnumerable<DBObject>> orderedGroups = GetOrderedGroups(dbObjects);
            foreach (IEnumerable<DBObject> group in orderedGroups.Reverse())
                orderedDbObjects.AddRange(group);
            return orderedDbObjects;
        }

        private static IEnumerable<IEnumerable<DBObject>> GetOrderedGroups(IEnumerable<DBObject> dbObjects)
        {
            List<IEnumerable<DBObject>> orderedGroups = new();
            HashSet<DBObject> uprocessedDbObjects = new(dbObjects);
            HashSet<Guid> processedDbObjects = new();
            Dictionary<Guid, HashSet<Guid>> isDependencyOfMap = CreateIsDependencyOfMap(dbObjects);
            AddDbObjectsThatAreNotDependencyOfAnyOtherUnprocessedDbObject(
                orderedGroups, uprocessedDbObjects, processedDbObjects, isDependencyOfMap);
            return orderedGroups;
        }

        private static void AddDbObjectsThatAreNotDependencyOfAnyOtherUnprocessedDbObject(
            List<IEnumerable<DBObject>> orderedGroups,
            HashSet<DBObject> uprocessedDbObjects,
            HashSet<Guid> processedDbObjects,
            Dictionary<Guid, HashSet<Guid>> isDependencyOfMap)
        {
            IEnumerable<DBObject> dbObjectsThatAreNotDependencyOfAnyOtherUnprocessedDbObject =
                uprocessedDbObjects.Where(dbObject =>
                {
                    bool dbObjectIsDependencyOfAnotherUnprocessedDbObject;
                    if (isDependencyOfMap.TryGetValue(dbObject.ID, out HashSet<Guid> isDependencyOf))
                        dbObjectIsDependencyOfAnotherUnprocessedDbObject = isDependencyOf.Except(processedDbObjects).Any();
                    else
                        dbObjectIsDependencyOfAnotherUnprocessedDbObject = false;
                    return !dbObjectIsDependencyOfAnotherUnprocessedDbObject;
                });

            IEnumerable<DBObject> newProcessedGroup = dbObjectsThatAreNotDependencyOfAnyOtherUnprocessedDbObject
                .OrderBy(x => x.ID)
                .ToList();

            if (!newProcessedGroup.Any())
                throw new Exception("Invalid objects dependencies graph, probably with cyclic dependency");

            orderedGroups.Add(newProcessedGroup);
            processedDbObjects.UnionWith(newProcessedGroup.Select(x => x.ID));
            uprocessedDbObjects.ExceptWith(newProcessedGroup);

            if (!uprocessedDbObjects.Any())
                return;

            AddDbObjectsThatAreNotDependencyOfAnyOtherUnprocessedDbObject(
                orderedGroups, uprocessedDbObjects, processedDbObjects, isDependencyOfMap);
        }

        private static Dictionary<Guid, HashSet<Guid>> CreateIsDependencyOfMap(IEnumerable<DBObject> dbObjects)
        {
            Dictionary<Guid, HashSet<Guid>> isDependencyOfMap = new();
            foreach (DBObject dbObject in dbObjects)
            {
                foreach (DBObject dep in dbObject.DependsOn)
                {
                    if (!isDependencyOfMap.ContainsKey(dep.ID))
                        isDependencyOfMap.Add(dep.ID, new HashSet<Guid>());
                    isDependencyOfMap[dep.ID].Add(dbObject.ID);
                }
            }
            return isDependencyOfMap;
        }
    }
}
