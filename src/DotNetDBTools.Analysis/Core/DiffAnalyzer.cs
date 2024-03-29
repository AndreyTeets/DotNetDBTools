﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Analysis.Core;

internal static class DiffAnalyzer
{
    public static bool IsEmpty(DatabaseDiff dbDiff)
    {
        IEnumerable<PropertyInfo> properties = GetProperties(dbDiff.GetType());
        return AllCollectionPropertiesAreEmpty(dbDiff, properties);
    }

    public static bool IsEmpty(DbObjectDiff dbObjectDiff)
    {
        IEnumerable<PropertyInfo> properties = GetProperties(dbObjectDiff.GetType());
        if (dbObjectDiff.NewName != dbObjectDiff.OldName)
            return false;
        foreach (PropertyInfo property in properties)
        {
            if (!IsCollection(property.PropertyType)
                && property.Name != nameof(DbObjectDiff.ID)
                && property.Name != nameof(DbObjectDiff.NewName)
                && property.Name != nameof(DbObjectDiff.OldName))
            {
                object propertyValue = property.GetValue(dbObjectDiff, null);
                if (propertyValue is not null)
                    return false;
            }
        }
        return AllCollectionPropertiesAreEmpty(dbObjectDiff, properties);
    }

    private static bool AllCollectionPropertiesAreEmpty(object obj, IEnumerable<PropertyInfo> properties)
    {
        foreach (PropertyInfo property in properties)
        {
            if (IsCollection(property.PropertyType))
            {
                IEnumerable collection = (IEnumerable)property.GetValue(obj, null);
                if (collection is null)
                    continue;
                foreach (object _ in collection)
                    return false;
            }
        }
        return true;
    }

    private static IEnumerable<PropertyInfo> GetProperties(Type type)
    {
        return type
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(x => x.CanRead && x.Name != "SyncRoot" && x.GetIndexParameters().Length == 0 &&
                x.Module.Assembly.FullName == typeof(Database).Assembly.FullName);
    }

    private static bool IsCollection(Type type)
    {
        return type != typeof(string) &&
            typeof(IEnumerable).IsAssignableFrom(type);
    }
}
