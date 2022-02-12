using System;
using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Analysis.Core
{
    public static class OrderingExtensions
    {
        public static List<TDbObject> OrderByName<TDbObject>(this IEnumerable<TDbObject> dbObjects)
            where TDbObject : DbObject
        {
            return dbObjects.OrderBy(x => x.Name, StringComparer.Ordinal).ToList();
        }

        public static IEnumerable<DbObject> OrderByDependenciesLast(this IEnumerable<DbObject> dbObjects)
        {
            if (!dbObjects.Any())
                return dbObjects;
            List<DbObject> orderedDbObjects = new();
            IEnumerable<IEnumerable<DbObject>> orderedGroups = GetOrderedGroups(dbObjects);
            foreach (IEnumerable<DbObject> group in orderedGroups)
                orderedDbObjects.AddRange(group);
            return orderedDbObjects;
        }

        public static IEnumerable<DbObject> OrderByDependenciesFirst(this IEnumerable<DbObject> dbObjects)
        {
            if (!dbObjects.Any())
                return dbObjects;
            List<DbObject> orderedDbObjects = new();
            IEnumerable<IEnumerable<DbObject>> orderedGroups = GetOrderedGroups(dbObjects);
            foreach (IEnumerable<DbObject> group in orderedGroups.Reverse())
                orderedDbObjects.AddRange(group);
            return orderedDbObjects;
        }

        private static IEnumerable<IEnumerable<DbObject>> GetOrderedGroups(IEnumerable<DbObject> dbObjects)
        {
            List<IEnumerable<DbObject>> orderedGroups = new();
            HashSet<DbObject> uprocessedDbObjects = new(dbObjects);
            HashSet<Guid> processedDbObjects = new();
            Dictionary<Guid, HashSet<Guid>> isDependencyOfMap = CreateIsDependencyOfMap(dbObjects);
            AddDbObjectsThatAreNotDependencyOfAnyOtherUnprocessedDbObject(
                orderedGroups, uprocessedDbObjects, processedDbObjects, isDependencyOfMap);
            return orderedGroups;
        }

        private static void AddDbObjectsThatAreNotDependencyOfAnyOtherUnprocessedDbObject(
            List<IEnumerable<DbObject>> orderedGroups,
            HashSet<DbObject> uprocessedDbObjects,
            HashSet<Guid> processedDbObjects,
            Dictionary<Guid, HashSet<Guid>> isDependencyOfMap)
        {
            IEnumerable<DbObject> dbObjectsThatAreNotDependencyOfAnyOtherUnprocessedDbObject =
                uprocessedDbObjects.Where(dbObject =>
                {
                    bool dbObjectIsDependencyOfAnotherUnprocessedDbObject;
                    if (isDependencyOfMap.TryGetValue(dbObject.ID, out HashSet<Guid> isDependencyOf))
                        dbObjectIsDependencyOfAnotherUnprocessedDbObject = isDependencyOf.Except(processedDbObjects).Any();
                    else
                        dbObjectIsDependencyOfAnotherUnprocessedDbObject = false;
                    return !dbObjectIsDependencyOfAnotherUnprocessedDbObject;
                });

            IEnumerable<DbObject> newProcessedGroup = dbObjectsThatAreNotDependencyOfAnyOtherUnprocessedDbObject
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

        private static Dictionary<Guid, HashSet<Guid>> CreateIsDependencyOfMap(IEnumerable<DbObject> dbObjects)
        {
            Dictionary<Guid, HashSet<Guid>> isDependencyOfMap = new();
            foreach (DbObject dbObject in dbObjects)
            {
                foreach (DbObject dep in dbObject.DependsOn)
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
