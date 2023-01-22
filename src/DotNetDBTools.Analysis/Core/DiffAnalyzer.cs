using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.PostgreSQL;
using DotNetDBTools.Models.PostgreSQL.UserDefinedTypes;

namespace DotNetDBTools.Analysis.Core;

internal static class DiffAnalyzer
{
    public static bool IsEmpty(DatabaseDiff dbDiff)
    {
        IEnumerable<PropertyInfo> properties = GetProperties(dbDiff.GetType());
        return AllCollectionPropertiesAreEmpty(dbDiff, properties);
    }

    public static bool IsEmpty(TableDiff tableDiff)
    {
        IEnumerable<PropertyInfo> properties = GetProperties(tableDiff.GetType());
        if (tableDiff.NewTableName != tableDiff.OldTableName || tableDiff.NewTable.Name != tableDiff.OldTable.Name)
            return false;
        foreach (PropertyInfo property in properties)
        {
            if (!IsCollection(property.PropertyType)
                && !typeof(Table).IsAssignableFrom(property.PropertyType)
                && property.Name != nameof(TableDiff.TableID)
                && property.Name != nameof(TableDiff.NewTableName)
                && property.Name != nameof(TableDiff.OldTableName))
            {
                object propertyValue = property.GetValue(tableDiff, null);
                if (propertyValue is not null)
                    return false;
            }
        }
        return AllCollectionPropertiesAreEmpty(tableDiff, properties);
    }

    public static bool IsEmpty(ColumnDiff columnDiff)
    {
        IEnumerable<PropertyInfo> properties = GetProperties(columnDiff.GetType());
        if (columnDiff.NewColumnName != columnDiff.OldColumnName)
            return false;
        foreach (PropertyInfo property in properties)
        {
            if (!IsCollection(property.PropertyType)
                && !typeof(Column).IsAssignableFrom(property.PropertyType)
                && property.Name != nameof(ColumnDiff.ColumnID)
                && property.Name != nameof(ColumnDiff.NewColumnName)
                && property.Name != nameof(ColumnDiff.OldColumnName))
            {
                object propertyValue = property.GetValue(columnDiff, null);
                if (propertyValue is not null)
                    return false;
            }
        }
        return AllCollectionPropertiesAreEmpty(columnDiff, properties);
    }

    public static bool IsEmpty(PostgreSQLSequenceDiff sequenceDiff)
    {
        IEnumerable<PropertyInfo> properties = GetProperties(sequenceDiff.GetType());
        if (sequenceDiff.NewSequenceName != sequenceDiff.OldSequenceName)
            return false;
        foreach (PropertyInfo property in properties)
        {
            if (!IsCollection(property.PropertyType)
                && property.Name != nameof(PostgreSQLSequenceDiff.SequenceID)
                && property.Name != nameof(PostgreSQLSequenceDiff.NewSequenceName)
                && property.Name != nameof(PostgreSQLSequenceDiff.OldSequenceName))
            {
                object propertyValue = property.GetValue(sequenceDiff, null);
                if (propertyValue is not null)
                    return false;
            }
        }
        return AllCollectionPropertiesAreEmpty(sequenceDiff, properties);
    }

    public static bool IsEmpty(PostgreSQLDomainTypeDiff typeDiff)
    {
        IEnumerable<PropertyInfo> properties = GetProperties(typeDiff.GetType());
        if (typeDiff.NewTypeName != typeDiff.OldTypeName)
            return false;
        foreach (PropertyInfo property in properties)
        {
            if (!IsCollection(property.PropertyType)
                && property.Name != nameof(PostgreSQLDomainTypeDiff.TypeID)
                && property.Name != nameof(PostgreSQLDomainTypeDiff.NewTypeName)
                && property.Name != nameof(PostgreSQLDomainTypeDiff.OldTypeName))
            {
                object propertyValue = property.GetValue(typeDiff, null);
                if (propertyValue is not null)
                    return false;
            }
        }
        return AllCollectionPropertiesAreEmpty(typeDiff, properties);
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
