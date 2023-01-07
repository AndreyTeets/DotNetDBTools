﻿using System;
using DotNetDBTools.Analysis;
using DotNetDBTools.Definition.Core;
using DotNetDBTools.Definition.Core.CSharpDataTypes;
using DotNetDBTools.Definition.MySQL;
using DotNetDBTools.DefinitionParsing.Core;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.DefinitionParsing.MySQL;

internal class MySQLDataTypeMapper : DataTypeMapper
{
    public override DataType MapToDataTypeModel(IDataType dataType)
    {
        switch (dataType)
        {
            case null:
                return new DataType();
            case IntDataType:
            case RealDataType:
            case DecimalDataType:
            case BoolDataType:

            case StringDataType:
            case BinaryDataType:
            case GuidDataType:

            case DateDataType:
            case TimeDataType:
            case DateTimeDataType:
                CSharpDataType csharpDataType = CreateCSharpDataTypeModel(dataType);
                return new AnalysisManager().ConvertDataType(csharpDataType, DatabaseKind.MySQL);

            case VerbatimDataType verbatimDataType:
                return new DataType { Name = verbatimDataType.Name };

            default:
                throw new InvalidOperationException($"Invalid dataType: {dataType}");
        }
    }
}
