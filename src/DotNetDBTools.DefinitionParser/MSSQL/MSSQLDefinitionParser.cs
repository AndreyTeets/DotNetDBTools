﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DotNetDBTools.Definition.MSSQL;
using DotNetDBTools.DefinitionParser.Core;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MSSQL;

namespace DotNetDBTools.DefinitionParser.MSSQL
{
    public class MSSQLDefinitionParser
    {
        public static MSSQLDatabaseInfo CreateDatabaseInfo(string dbAssemblyPath)
        {
            Assembly dbAssembly = AssemblyLoader.LoadDbAssemblyFromDll(dbAssemblyPath);
            return CreateDatabaseInfo(dbAssembly);
        }

        public static MSSQLDatabaseInfo CreateDatabaseInfo(Assembly dbAssembly)
        {
            IEnumerable<ITable> tables = dbAssembly.GetTypes()
                .Where(x => x.GetInterfaces()
                    .Any(y => y == typeof(ITable)))
                .OrderBy(x => x.Name, StringComparer.Ordinal)
                .Select(x => (ITable)Activator.CreateInstance(x));

            IEnumerable<IView> views = dbAssembly.GetTypes()
                .Where(x => x.GetInterfaces()
                    .Any(y => y == typeof(IView)))
                .OrderBy(x => x.Name, StringComparer.Ordinal)
                .Select(x => (IView)Activator.CreateInstance(x));

            IEnumerable<IFunction> functions = dbAssembly.GetTypes()
                .Where(x => x.GetInterfaces()
                    .Any(y => y == typeof(IFunction)))
                .OrderBy(x => x.Name, StringComparer.Ordinal)
                .Select(x => (IFunction)Activator.CreateInstance(x));

            IEnumerable<IUserDefinedType> userDefinedTypes = dbAssembly.GetTypes()
                .Where(x => x.GetInterfaces()
                    .Any(y => y == typeof(IUserDefinedType)))
                .OrderBy(x => x.Name, StringComparer.Ordinal)
                .Select(x => (IUserDefinedType)Activator.CreateInstance(x));

            return new MSSQLDatabaseInfo(DbAssemblyInfoHelper.GetDbName(dbAssembly))
            {
                Tables = GetTableInfos(tables),
                Views = GetViewInfos(views),
                Functions = GetFunctionInfos(functions),
                UserDefinedTypes = GetUserDefinedTypesInfos(userDefinedTypes),
            };
        }

        private static List<MSSQLTableInfo> GetTableInfos(IEnumerable<ITable> tables)
        {
            List<MSSQLTableInfo> tableInfos = new();
            foreach (ITable table in tables)
            {
                MSSQLTableInfo tableInfo = new()
                {
                    ID = table.ID,
                    Name = table.GetType().Name,
                    Columns = GetColumnInfos(table),
                    PrimaryKey = GetPrimaryKeyInfo(table),
                    UniqueConstraints = GetUniqueConstraintsInfos(table),
                    ForeignKeys = GetForeignKeyInfos(table),
                };
                tableInfos.Add(tableInfo);
            }
            return tableInfos;
        }

        private static List<MSSQLViewInfo> GetViewInfos(IEnumerable<IView> views)
        {
            List<MSSQLViewInfo> viewInfos = new();
            foreach (IView view in views)
            {
                MSSQLViewInfo viewInfo = new()
                {
                    ID = view.ID,
                    Name = view.GetType().Name,
                    Code = view.Code,
                };
                viewInfos.Add(viewInfo);
            }
            return viewInfos;
        }

        private static List<MSSQLFunctionInfo> GetFunctionInfos(IEnumerable<IFunction> functions)
        {
            List<MSSQLFunctionInfo> functionInfos = new();
            foreach (IFunction function in functions)
            {
                MSSQLFunctionInfo functionInfo = new()
                {
                    ID = function.ID,
                    Name = function.GetType().Name,
                    Code = function.Code,
                };
                functionInfos.Add(functionInfo);
            }
            return functionInfos;
        }

        private static List<MSSQLUserDefinedTypeInfo> GetUserDefinedTypesInfos(IEnumerable<IUserDefinedType> userDefinedTypes)
        {
            List<MSSQLUserDefinedTypeInfo> userDefinedTypeInfos = new();
            foreach (IUserDefinedType userDefinedType in userDefinedTypes)
            {
                DataTypeInfo dataTypeInfo = MSSQLDataTypeMapper.GetDataTypeInfo(userDefinedType.UnderlyingType);
                MSSQLUserDefinedTypeInfo userDefinedTypeInfo = new()
                {
                    ID = userDefinedType.ID,
                    Name = userDefinedType.GetType().Name,
                    Nullable = userDefinedType.Nullable,
                    UnderlyingDataType = dataTypeInfo,
                };
                userDefinedTypeInfos.Add(userDefinedTypeInfo);
            }
            return userDefinedTypeInfos;
        }

        private static List<ColumnInfo> GetColumnInfos(ITable table)
            => table.GetType().GetPropertyOrFieldMembers()
                .Where(x => typeof(Column).IsAssignableFrom(x.GetPropertyOrFieldType()))
                .OrderBy(x => x.Name, StringComparer.Ordinal)
                .Select(x =>
                {
                    Column column = (Column)x.GetPropertyOrFieldValue(table);
                    DataTypeInfo dataTypeInfo = MSSQLDataTypeMapper.GetDataTypeInfo(column.DataType);
                    return new ColumnInfo()
                    {
                        ID = column.ID,
                        Name = x.Name,
                        DataType = dataTypeInfo,
                        Nullable = column.Nullable,
                        Identity = column.Identity,
                        Default = MSSQLDefaultValueMapper.MapDefaultValue(column),
                    };
                })
                .ToList();

        private static PrimaryKeyInfo GetPrimaryKeyInfo(ITable table)
            => table.GetType().GetPropertyOrFieldMembers()
                .Where(x => typeof(PrimaryKey).IsAssignableFrom(x.GetPropertyOrFieldType()))
                .OrderBy(x => x.Name, StringComparer.Ordinal)
                .Select(x =>
                {
                    PrimaryKey primaryKey = (PrimaryKey)x.GetPropertyOrFieldValue(table);
                    return new PrimaryKeyInfo()
                    {
                        ID = primaryKey.ID,
                        Name = x.Name,
                        Columns = primaryKey.Columns.ToList(),
                    };
                })
                .SingleOrDefault();

        private static List<UniqueConstraintInfo> GetUniqueConstraintsInfos(ITable table)
            => table.GetType().GetPropertyOrFieldMembers()
                .Where(x => typeof(UniqueConstraint).IsAssignableFrom(x.GetPropertyOrFieldType()))
                .OrderBy(x => x.Name, StringComparer.Ordinal)
                .Select(x =>
                {
                    UniqueConstraint uniqueConstraint = (UniqueConstraint)x.GetPropertyOrFieldValue(table);
                    return new UniqueConstraintInfo()
                    {
                        ID = uniqueConstraint.ID,
                        Name = x.Name,
                        Columns = uniqueConstraint.Columns.ToList(),
                    };
                })
                .ToList();

        private static List<ForeignKeyInfo> GetForeignKeyInfos(ITable table)
            => table.GetType().GetPropertyOrFieldMembers()
                .Where(x => typeof(ForeignKey).IsAssignableFrom(x.GetPropertyOrFieldType()))
                .OrderBy(x => x.Name, StringComparer.Ordinal)
                .Select(x =>
                {
                    ForeignKey foreignKey = (ForeignKey)x.GetPropertyOrFieldValue(table);
                    return new ForeignKeyInfo()
                    {
                        ID = foreignKey.ID,
                        Name = x.Name,
                        ThisTableName = table.GetType().Name,
                        ThisColumnNames = foreignKey.ThisColumns.ToList(),
                        ForeignTableName = foreignKey.ForeignTable,
                        ForeignColumnNames = foreignKey.ForeignColumns.ToList(),
                        OnDelete = foreignKey.OnDelete.ToString(),
                        OnUpdate = foreignKey.OnUpdate.ToString(),
                    };
                })
                .ToList();
    }
}