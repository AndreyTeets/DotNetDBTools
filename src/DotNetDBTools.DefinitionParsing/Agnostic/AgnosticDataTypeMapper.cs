using System;
using DotNetDBTools.Definition.Agnostic.DataTypes;
using DotNetDBTools.Definition.Core;
using DotNetDBTools.DefinitionParsing.Core;
using DotNetDBTools.Models.Agnostic;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.DefinitionParsing.Agnostic;

internal class AgnosticDataTypeMapper : IDataTypeMapper
{
    public DataType MapToDataTypeModel(IDataType dataType)
    {
        return dataType switch
        {
            IntDataType dt => MapToDataTypeModel(dt),
            RealDataType dt => MapToDataTypeModel(dt),
            DecimalDataType dt => MapToDataTypeModel(dt),
            BoolDataType dt => new AgnosticDataType() { Name = AgnosticDataTypeNames.Bool },

            StringDataType dt => MapToDataTypeModel(dt),
            BinaryDataType dt => MapToDataTypeModel(dt),
            GuidDataType dt => new AgnosticDataType() { Name = AgnosticDataTypeNames.Guid },

            DateDataType dt => new AgnosticDataType() { Name = AgnosticDataTypeNames.Date },
            TimeDataType dt => new AgnosticDataType() { Name = AgnosticDataTypeNames.Time },
            DateTimeDataType dt => MapToDataTypeModel(dt),

            _ => throw new InvalidOperationException($"Invalid dataType: '{dataType}'")
        };
    }

    private static AgnosticDataType MapToDataTypeModel(IntDataType intDataType)
    {
        return new AgnosticDataType()
        {
            Name = AgnosticDataTypeNames.Int,
            Size = int.Parse($"{intDataType.Size}".Replace("Int", "")),
        };
    }

    private static AgnosticDataType MapToDataTypeModel(RealDataType realDataType)
    {
        return new AgnosticDataType()
        {
            Name = AgnosticDataTypeNames.Real,
            IsDoublePrecision = realDataType.IsDoublePrecision,
        };
    }

    private static AgnosticDataType MapToDataTypeModel(DecimalDataType decimalDataType)
    {
        return new AgnosticDataType()
        {
            Name = AgnosticDataTypeNames.Decimal,
            Precision = decimalDataType.Precision,
            Scale = decimalDataType.Scale,
        };
    }

    private static AgnosticDataType MapToDataTypeModel(StringDataType stringDataType)
    {
        return new AgnosticDataType()
        {
            Name = AgnosticDataTypeNames.String,
            Length = stringDataType.Length,
            IsFixedLength = stringDataType.IsFixedLength,
        };
    }

    private static AgnosticDataType MapToDataTypeModel(BinaryDataType binaryDataType)
    {
        return new AgnosticDataType()
        {
            Name = AgnosticDataTypeNames.Binary,
            Length = binaryDataType.Length,
            IsFixedLength = binaryDataType.IsFixedLength,
        };
    }

    private static AgnosticDataType MapToDataTypeModel(DateTimeDataType dateTimeDataType)
    {
        return new AgnosticDataType()
        {
            Name = AgnosticDataTypeNames.DateTime,
            IsWithTimeZone = dateTimeDataType.IsWithTimeZone,
        };
    }
}
