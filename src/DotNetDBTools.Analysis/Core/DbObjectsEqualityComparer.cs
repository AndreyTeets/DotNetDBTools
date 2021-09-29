using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Analysis.Core
{
    internal class DbObjectsEqualityComparer : IEqualityComparer<DBObjectInfo>
    {
        public bool Equals(DBObjectInfo x, DBObjectInfo y)
        {
            if (x is null && y is null)
                return true;
            else if (x is null || y is null)
                return false;

            return HaveEqualPropertiesRecursive(x, y);
        }

        public int GetHashCode(DBObjectInfo obj)
        {
            return obj.GetHashCode();
        }

        private static bool HaveEqualPropertiesRecursive<T>(T first, T second)
            where T : class
        {
            Type parentType = first.GetType();
            IEnumerable properties = parentType
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.CanRead && x.Name != "SyncRoot" && x.GetIndexParameters().Length == 0 &&
                    x.Module.Assembly.FullName == typeof(DatabaseInfo).Assembly.FullName);

            foreach (PropertyInfo property in properties)
            {
                object value1 = property.GetValue(first, null);
                object value2 = property.GetValue(second, null);
                if (value1 is null && value2 is null)
                    continue;
                else if (value1 is null || value2 is null)
                    return false;

                if (IsSimpleObject(property.PropertyType))
                {
                    if (SimpleObjectsAreEqual(value1, value2))
                        continue;
                    else
                        return false;
                }
                else if (IsCollection(property.PropertyType))
                {
                    if (CollectionsAreEqual((IEnumerable)value1, (IEnumerable)value2))
                        continue;
                    else
                        return false;
                }
                else
                {
                    Type value1Type = value1.GetType();
                    Type value2Type = value2.GetType();
                    if (value1Type != value2Type)
                        return false;

                    if (IsSimpleObject(value1Type))
                    {
                        if (SimpleObjectsAreEqual(value1, value2))
                            continue;
                        else
                            return false;
                    }

                    if (HaveEqualPropertiesRecursive(value1, value2))
                        continue;
                    else
                        return false;
                }
            }

            return true;
        }

        private static bool IsSimpleObject(Type type)
        {
            return type.IsValueType ||
                type == typeof(string) ||
                type == typeof(Guid);
        }

        private static bool IsCollection(Type type)
        {
            return type != typeof(string) &&
                typeof(IEnumerable).IsAssignableFrom(type);
        }

        private static bool SimpleObjectsAreEqual(object obj1, object obj2)
        {
            return obj1.Equals(obj2);
        }

        private static bool CollectionsAreEqual(IEnumerable collection1, IEnumerable collection2)
        {
            CollectionItemsEqualityComparer comparer = new();
            Dictionary<object, int> collection1ObjectsCounts = new(comparer);
            Dictionary<object, int> collection2ObjectsCounts = new(comparer);

            foreach (object item in collection1)
                AddOrIncrement(collection1ObjectsCounts, item);
            foreach (object item in collection2)
                AddOrIncrement(collection2ObjectsCounts, item);

            if (collection1ObjectsCounts.Count() != collection2ObjectsCounts.Count())
                return false;

            foreach (object key in collection1ObjectsCounts.Keys)
            {
                if (!collection2ObjectsCounts.ContainsKey(key))
                    return false;

                int keyCountInCollection1 = collection1ObjectsCounts[key];
                int keyCountInCollection2 = collection2ObjectsCounts[key];
                if (keyCountInCollection1 != keyCountInCollection2)
                    return false;
            }

            return true;

            void AddOrIncrement(Dictionary<object, int> objectsCounts, object item)
            {
                if (objectsCounts.ContainsKey(item))
                    objectsCounts[item]++;
                else
                    objectsCounts[item] = 1;
            }
        }

        private class CollectionItemsEqualityComparer : IEqualityComparer<object>
        {
            public new bool Equals(object x, object y)
            {
                if (x is null && y is null)
                    return true;
                else if (x is null || y is null)
                    return false;

                Type xType = x.GetType();
                Type yType = y.GetType();
                if (xType != yType)
                    return false;

                if (IsSimpleObject(xType))
                    return SimpleObjectsAreEqual(x, y);
                else if (typeof(DBObjectInfo).IsAssignableFrom(xType))
                    return HaveEqualPropertiesRecursive(x, y);
                else
                    throw new InvalidOperationException($"Invalid item type '{xType}' in collection");
            }

            public int GetHashCode(object obj)
            {
                Type type = obj.GetType();
                if (IsSimpleObject(type))
                    return obj.GetHashCode();
                else if (typeof(DBObjectInfo).IsAssignableFrom(type))
                    return ((DBObjectInfo)obj).ID.GetHashCode();
                else
                    throw new InvalidOperationException($"Invalid item type '{type}' in collection");
            }
        }
    }
}
