using System;
using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Analysis.Extensions;

public static class DbObjectsExtensions
{
    /// <summary>
    /// Sorts by DbObject.Name ascending (ordinal) then by DbObject.GetType().Name.
    /// </summary>
    public static IEnumerable<TDbObject> OrderByNameThenByType<TDbObject>(this IEnumerable<TDbObject> dbObjects)
        where TDbObject : DbObject
    {
        return dbObjects.OrderBy(x => x.Name, StringComparer.Ordinal).ThenBy(x => x.GetType().Name).ToList();
    }

    /// <summary>
    /// Gets all object's transitive dependencies using a given rule to choose object's direct dependencies.
    /// </summary>
    public static IEnumerable<DbObject> GetTransitiveDependencies(
        this DbObject dbObject, Func<DbObject, IEnumerable<DbObject>> getDependenciesFunc)
    {
        Dictionary<Guid, DbObject> transitiveDeps = new();
        AddDependenciesTransitively(transitiveDeps, getDependenciesFunc(dbObject));
        return transitiveDeps.Values.OrderByNameThenByType();

        void AddDependenciesTransitively(Dictionary<Guid, DbObject> targetTransitiveDeps, IEnumerable<DbObject> sourceDeps)
        {
            foreach (DbObject dep in sourceDeps.Where(x => !targetTransitiveDeps.ContainsKey(x.ID)))
            {
                targetTransitiveDeps.Add(dep.ID, dep);
                AddDependenciesTransitively(targetTransitiveDeps, getDependenciesFunc(dep));
            }
        }
    }

    /// <summary>
    /// Sorts so that for every DbObject all objects in DbObject.DependsOn appear after it.
    /// Meaning that for every DbObject all objects in DbObject.IsDependencyOf appear before it.
    /// Throws on recursive dependency.
    /// </summary>
    public static IEnumerable<DbObject> OrderByDependenciesLast(
        this IEnumerable<DbObject> dbObjects, Func<DbObject, IEnumerable<DbObject>> getDependenciesFunc)
    {
        if (!dbObjects.Any())
            return dbObjects;
        List<DbObject> orderedDbObjects = new();
        IEnumerable<IEnumerable<DbObject>> orderedGroups = GetOrderedGroups(dbObjects, getDependenciesFunc);
        foreach (IEnumerable<DbObject> group in orderedGroups)
            orderedDbObjects.AddRange(group.OrderByNameThenByType());
        return orderedDbObjects;
    }

    /// <summary>
    /// Sorts so that for every DbObject all objects in DbObject.DependsOn appear before it.
    /// Throws on recursive dependency.
    /// </summary>
    public static IEnumerable<DbObject> OrderByDependenciesFirst(
        this IEnumerable<DbObject> dbObjects, Func<DbObject, IEnumerable<DbObject>> getDependenciesFunc)
    {
        if (!dbObjects.Any())
            return dbObjects;
        List<DbObject> orderedDbObjects = new();
        IEnumerable<IEnumerable<DbObject>> orderedGroups = GetOrderedGroups(dbObjects, getDependenciesFunc);
        foreach (IEnumerable<DbObject> group in orderedGroups.Reverse())
            orderedDbObjects.AddRange(group.OrderByNameThenByType());
        return orderedDbObjects;
    }

    private static IEnumerable<IEnumerable<DbObject>> GetOrderedGroups(
        IEnumerable<DbObject> dbObjects, Func<DbObject, IEnumerable<DbObject>> getDependenciesFunc)
    {
        List<IEnumerable<DbObject>> orderedGroups = new();
        HashSet<DbObject> uprocessedDbObjects = new(dbObjects);
        HashSet<Guid> processedDbObjects = new();
        Dictionary<Guid, HashSet<Guid>> isDependencyOfMap = CreateIsDependencyOfMap(dbObjects, getDependenciesFunc);
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

        // TODO x=ObjectsToChooseWhenOnlyCyclicGraphLeft(uprocessedDbObjects); if (x.Count==0) throw else newProcessedGroup=x
        // with default one return empty list. This may be required to resolve e.g. cyclic views creation (e.g. v1->v2->v1)
        // via e.g. CREATE v1 AS RETURN NULL::<RetType>;CREATE v2;CREATE OR REPLACE v1;
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

    private static Dictionary<Guid, HashSet<Guid>> CreateIsDependencyOfMap(
        IEnumerable<DbObject> dbObjects, Func<DbObject, IEnumerable<DbObject>> getDependenciesFunc)
    {
        Dictionary<Guid, HashSet<Guid>> isDependencyOfMap = new();
        foreach (DbObject dbObject in dbObjects)
        {
            foreach (DbObject dep in dbObject.GetTransitiveDependencies(getDependenciesFunc))
            {
                if (!isDependencyOfMap.ContainsKey(dep.ID))
                    isDependencyOfMap.Add(dep.ID, new HashSet<Guid>());
                isDependencyOfMap[dep.ID].Add(dbObject.ID);
            }
        }
        return isDependencyOfMap;
    }
}
