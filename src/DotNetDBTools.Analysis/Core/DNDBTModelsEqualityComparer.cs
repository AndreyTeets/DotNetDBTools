using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using DotNetDBTools.Analysis.Extensions;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Analysis.Core;

internal class DNDBTModelsEqualityComparer : IEqualityComparer<object>
{
    public HashSet<PropInfo> IgnoredProperties { get; set; } = new();
    public string DiffLog => _diffInfoSb.ToString().NormalizeLineEndings().TrimEnd();

    private readonly StringBuilder _diffInfoSb = new();
    private readonly HashSet<PropInfo> _alwaysIgnoredProperties = new()
    {
        new PropInfo { Name = nameof(DbObject.Parent), InType = nameof(DbObject) },
        new PropInfo { Name = nameof(CodePiece.DependsOn), InType = nameof(CodePiece) },
        new PropInfo { Name = nameof(DataType.DependsOn), InType = nameof(DataType) },
        new PropInfo { Name = nameof(PrimaryKey.DependsOn), InType = nameof(PrimaryKey) },
        new PropInfo { Name = nameof(UniqueConstraint.DependsOn), InType = nameof(UniqueConstraint) },
        new PropInfo { Name = nameof(ForeignKey.DependsOn), InType = nameof(ForeignKey) },
        new PropInfo { Name = nameof(Index.DependsOn), InType = nameof(Index) },
    };

    public new bool Equals(object x, object y)
    {
        if (x is null && y is null)
            return true;
        else if (x is null || y is null)
            return LogDiffAndReturnFalse(x, y, $"OneNullOfObjectBeingCompared");

        Type xType = x.GetType();
        Type yType = y.GetType();
        if (xType != yType)
            return LogDiffAndReturnFalse(xType, yType, $"TypeOfObjectBeingCompared");

        if (IsSimpleObject(xType))
            return SimpleObjectsAreEqual(x, y);
        else
            return HaveEqualPropertiesRecursive(x, y);
    }

    public int GetHashCode(object obj)
    {
        Type type = obj.GetType();
        if (IsSimpleObject(type))
            return obj.GetHashCode();
        else
            return GetAggregatedSimplePropertiesHashCode(obj);
    }

    private bool HaveEqualPropertiesRecursive<T>(T first, T second)
        where T : class
    {
        Type parentType = first.GetType();
        IEnumerable<PropertyInfo> properties = GetProperties(parentType);

        foreach (PropertyInfo property in properties)
        {
            if (IsIgnoredProperty(property))
                continue;

            object value1 = property.GetValue(first, null);
            object value2 = property.GetValue(second, null);
            if (value1 is null && value2 is null)
                continue;
            else if (value1 is null || value2 is null)
                return LogDiffAndReturnFalse(value1, value2, $"OneNullOfValueOfProperty:{property.Name}");

            if (IsSimpleObject(property.PropertyType))
            {
                if (!SimpleObjectsAreEqual(value1, value2))
                    return LogDiffAndReturnFalse(value1, value2, $"ValueOfSimpleProperty:{property.Name}");
            }
            else if (IsCollection(property.PropertyType))
            {
                if (!CollectionsAreEqual((IEnumerable)value1, (IEnumerable)value2))
                    return LogDiffAndReturnFalse(value1, value2, $"ItemsOfCollectionProperty:{property.Name}");
            }
            else
            {
                Type value1Type = value1.GetType();
                Type value2Type = value2.GetType();
                if (value1Type != value2Type)
                    return LogDiffAndReturnFalse(value1Type, value2Type, $"TypeOfComplexProperty:{property.Name}");

                if (IsSimpleObject(value1Type))
                {
                    if (!SimpleObjectsAreEqual(value1, value2))
                        return LogDiffAndReturnFalse(value1Type, value2Type, $"SimpleValueOfComplexProperty:{property.Name}");
                }
                else
                {
                    if (!HaveEqualPropertiesRecursive(value1, value2))
                        return LogDiffAndReturnFalse(value1, value2, $"ComplexValueOfComplexProperty:{property.Name}");
                }
            }
        }

        return true;
    }

    private IEnumerable<PropertyInfo> GetProperties(Type type)
    {
        return type
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(x => x.CanRead && x.Name != "SyncRoot" && x.GetIndexParameters().Length == 0 &&
                x.Module.Assembly.FullName == typeof(Database).Assembly.FullName);
    }

    private int GetAggregatedSimplePropertiesHashCode(object obj)
    {
        IEnumerable<PropertyInfo> properties = GetProperties(obj.GetType());
        long propertiesHashCodesSum = 0;
        foreach (PropertyInfo property in properties)
        {
            if (IsIgnoredProperty(property))
                continue;

            if (IsSimpleObject(property.PropertyType))
            {
                object propValue = property.GetValue(obj, null);
                if (propValue is not null)
                    propertiesHashCodesSum += propValue.GetHashCode();
            }
        }
        return propertiesHashCodesSum.GetHashCode();
    }

    private bool IsSimpleObject(Type type)
    {
        return type.IsValueType ||
            type == typeof(string) ||
            type == typeof(Guid);
    }

    private bool IsCollection(Type type)
    {
        return type != typeof(string) &&
            typeof(IEnumerable).IsAssignableFrom(type);
    }

    private bool SimpleObjectsAreEqual(object obj1, object obj2)
    {
        return obj1.Equals(obj2);
    }

    private bool CollectionsAreEqual(IEnumerable collection1, IEnumerable collection2)
    {
        Dictionary<object, int> collection1ObjectsCounts = new(this);
        Dictionary<object, int> collection2ObjectsCounts = new(this);

        foreach (object item in collection1)
            AddOrIncrement(collection1ObjectsCounts, item);
        foreach (object item in collection2)
            AddOrIncrement(collection2ObjectsCounts, item);

        if (collection1ObjectsCounts.Count != collection2ObjectsCounts.Count)
            return LogDiffAndReturnFalse(collection1ObjectsCounts.Count, collection2ObjectsCounts.Count, "CountOfUniqueItems");

        foreach (object key in collection1ObjectsCounts.Keys)
        {
            if (!collection2ObjectsCounts.ContainsKey(key))
                return LogDiffAndReturnFalse(key, null, "MissingItem");

            int keyCountInCollection1 = collection1ObjectsCounts[key];
            int keyCountInCollection2 = collection2ObjectsCounts[key];
            if (keyCountInCollection1 != keyCountInCollection2)
                return LogDiffAndReturnFalse(keyCountInCollection1, keyCountInCollection2, $"CountOfItem:{key}");
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

    private bool IsIgnoredProperty(PropertyInfo property)
    {
        return _alwaysIgnoredProperties.Any(Match) || IgnoredProperties.Any(Match);

        bool Match(PropInfo propInfo)
        {
            return propInfo.Name == property.Name
                && (propInfo.InType is null || propInfo.InType == property.DeclaringType.Name);
        }
    }

    private bool LogDiffAndReturnFalse(object val1, object val2, string place)
    {
        _diffInfoSb.AppendLine($"Values [{val1}] and [{val2}] differ at place [{place}].");
        return false;
    }
}
