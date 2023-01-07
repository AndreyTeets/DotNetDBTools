using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DotNetDBTools.Analysis.Core;
using DotNetDBTools.Analysis.Extensions;
using DotNetDBTools.CodeParsing;
using DotNetDBTools.CodeParsing.Models;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.PostgreSQL;
using DotNetDBTools.Models.PostgreSQL.UserDefinedTypes;

namespace DotNetDBTools.Analysis.PostgreSQL;

internal class PostgreSQLDbModelPostProcessor : DbModelPostProcessor
{
    public override void DoSpecificDbmsDbModelCreationFromDefinitionPostProcessing(Database database)
    {
        PostgreSQLDatabase db = (PostgreSQLDatabase)database;
        AddFunctionModelsFromTriggersCodeIfAny(db);
    }

    protected override void PostProcessDataTypes(Database database)
    {
        PostgreSQLDatabase db = (PostgreSQLDatabase)database;
        IEnumerable<DbObject> userDefinedTypes =
            db.CompositeTypes.Select(x => (DbObject)x)
            .Concat(db.DomainTypes.Select(x => (DbObject)x))
            .Concat(db.EnumTypes.Select(x => (DbObject)x))
            .Concat(db.RangeTypes.Select(x => (DbObject)x));

        HashSet<string> userDefinedTypesNames = new();
        foreach (DbObject udt in userDefinedTypes)
            userDefinedTypesNames.Add(udt.Name);

        foreach (Table table in db.Tables)
        {
            foreach (Column column in table.Columns)
            {
                PostProcessDataType(column.DataType);
            }
        }
        foreach (PostgreSQLDomainType type in db.DomainTypes)
        {
            PostProcessDataType(type.UnderlyingType);
        }
        foreach (PostgreSQLCompositeType type in db.CompositeTypes)
        {
            foreach (PostgreSQLCompositeTypeAttribute attr in type.Attributes)
            {
                PostProcessDataType(attr.DataType);
            }
        }
        foreach (PostgreSQLRangeType type in db.RangeTypes)
        {
            PostProcessDataType(type.Subtype);
        }

        void PostProcessDataType(DataType dataType)
        {
            if (dataType is null)
                return; // TODO move "Unknown data type" to DbValidator check as well

            dataType.Name = Regex.Replace(dataType.Name, @"\s", "");
            if (!userDefinedTypesNames.Contains(dataType.Name) && IsStandardSqlType(dataType.Name))
                dataType.Name = dataType.Name.ToUpper();
            else if (userDefinedTypesNames.Contains(dataType.Name))
                dataType.IsUserDefined = true;
            else
                throw new Exception($"Unknown data type '{dataType.Name}'");

            static bool IsStandardSqlType(string typeName)
            {
                IEnumerable<string> standardSqlTypeNamesBases = typeof(PostgreSQLDataTypeNames).GetFields().Select(x => x.Name);
                return standardSqlTypeNamesBases.Any(typeNameBase => TypeNameMatchesItsBase(typeName, typeNameBase));

                static bool TypeNameMatchesItsBase(string typeName, string typeNameBase)
                {
                    return Regex.IsMatch(typeName.ToUpper(), $@"^{typeNameBase}\s*(?:[\(][\s\d\,\-]+[\)])?\s*(?:[\[][\s\d]*[\]])*$");
                }
            }
        }
    }

    protected override void DoAdditionalPostProcessing(Database database)
    {
        PostgreSQLDatabase db = (PostgreSQLDatabase)database;
        foreach (PostgreSQLRangeType type in db.RangeTypes)
        {
            if (type.SubtypeOperatorClass is null)
                type.SubtypeOperatorClass = GetDefaultSubtypeOperatorClass(type.Subtype);
            if (type.Collation is null)
                type.Collation = GetDefaultCollation(type.Subtype);
            if (type.MultirangeTypeName is null)
                type.MultirangeTypeName = $"{type.Name}_multirange";
        }

        string GetDefaultSubtypeOperatorClass(DataType subtype)
        {
            return subtype.Name switch
            {
                PostgreSQLDataTypeNames.SMALLINT => "int2_ops",
                PostgreSQLDataTypeNames.INT => "int4_ops",
                PostgreSQLDataTypeNames.BIGINT => "int8_ops",
                PostgreSQLDataTypeNames.FLOAT4 => "float4_ops",
                PostgreSQLDataTypeNames.FLOAT8 => "float8_ops",
                PostgreSQLDataTypeNames.DECIMAL => "numeric_ops",
                PostgreSQLDataTypeNames.BOOL => "bool_ops",
                PostgreSQLDataTypeNames.MONEY => "money_ops",
                PostgreSQLDataTypeNames.CHAR => "char_ops",
                PostgreSQLDataTypeNames.VARCHAR => "text_ops",
                PostgreSQLDataTypeNames.TEXT => "text_ops",
                PostgreSQLDataTypeNames.BYTEA => "bytea_ops",
                PostgreSQLDataTypeNames.DATE => "date_ops",
                PostgreSQLDataTypeNames.TIME => "time_ops",
                PostgreSQLDataTypeNames.TIMETZ => "timetz_ops",
                PostgreSQLDataTypeNames.TIMESTAMP => "timestamp_ops",
                PostgreSQLDataTypeNames.TIMESTAMPTZ => "timestamptz_ops",
                PostgreSQLDataTypeNames.UUID => "uuid_ops",
                PostgreSQLDataTypeNames.BIT => "bit_ops",
                PostgreSQLDataTypeNames.VARBIT => "varbit_ops",
                _ => null,
            };
        }

        string GetDefaultCollation(DataType subtype)
        {
            return subtype.Name switch
            {
                PostgreSQLDataTypeNames.CHAR => "default",
                PostgreSQLDataTypeNames.VARCHAR => "default",
                PostgreSQLDataTypeNames.TEXT => "default",
                _ => null,
            };
        }
    }

    protected override void OrderAdditionalDbObjects(Database database)
    {
        PostgreSQLDatabase db = (PostgreSQLDatabase)database;

        db.CompositeTypes = db.CompositeTypes.OrderByName();
        db.DomainTypes = db.DomainTypes.OrderByName();
        db.EnumTypes = db.EnumTypes.OrderByName();
        db.RangeTypes = db.RangeTypes.OrderByName();

        db.Functions = db.Functions.OrderByName();
        db.Procedures = db.Procedures.OrderByName();
    }

    private void AddFunctionModelsFromTriggersCodeIfAny(PostgreSQLDatabase database)
    {
        foreach (Table table in database.Tables)
        {
            foreach (Trigger trg in table.Triggers)
            {
                List<string> statements = PostgreSQLStatementsSplitter.Split(trg.CreateStatement.Code);
                if (statements.Count == 1)
                    continue;

                if (statements.Count == 2)
                {
                    PostgreSQLCodeParser parser = new();
                    ObjectInfo objectInfo = parser.GetObjectInfo(statements[0]);
                    if (objectInfo is FunctionInfo func)
                    {
                        PostgreSQLFunction funcModel = new()
                        {
                            ID = func.ID.Value,
                            Name = func.Name,
                            CreateStatement = new CodePiece { Code = func.CreateStatement.NormalizeLineEndings() },
                        };
                        database.Functions.Add(funcModel);
                        trg.CreateStatement.Code = statements[1].NormalizeLineEndings();
                    }
                    else
                    {
                        throw new Exception($"Trigger '{trg.Name}' code contains 2 statements and first one is not a function");
                    }
                }
                else
                {
                    throw new Exception($"Found invalid count({statements.Count}) of statements in trigger code [{trg.CreateStatement.Code}]");
                }
            }
        }
    }
}
