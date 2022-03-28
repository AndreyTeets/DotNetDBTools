using System;
using DotNetDBTools.Definition.Core;
using DotNetDBTools.Definition.Core.CSharpDataTypes;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.DefinitionParsing.Core;

internal abstract class DataTypeMapper : IDataTypeMapper
{
    public abstract DataType MapToDataTypeModel(IDataType dataType);

    protected CSharpDataType CreateCSharpDataTypeModel(IDataType dataType)
    {
        return dataType switch
        {
            IntDataType dt => CreateCSharpDataTypeModel(dt),
            RealDataType dt => CreateCSharpDataTypeModel(dt),
            DecimalDataType dt => CreateCSharpDataTypeModel(dt),
            BoolDataType dt => new CSharpDataType() { Name = CSharpDataTypeNames.Bool },

            StringDataType dt => CreateCSharpDataTypeModel(dt),
            BinaryDataType dt => CreateCSharpDataTypeModel(dt),
            GuidDataType dt => new CSharpDataType() { Name = CSharpDataTypeNames.Guid },

            DateDataType dt => new CSharpDataType() { Name = CSharpDataTypeNames.Date },
            TimeDataType dt => new CSharpDataType() { Name = CSharpDataTypeNames.Time },
            DateTimeDataType dt => CreateCSharpDataTypeModel(dt),

            _ => throw new InvalidOperationException($"Invalid dataType: '{dataType}'")
        };
    }

    private static CSharpDataType CreateCSharpDataTypeModel(IntDataType intDataType)
    {
        return new CSharpDataType()
        {
            Name = CSharpDataTypeNames.Int,
            Size = int.Parse($"{intDataType.Size}".Replace("Int", "")),
        };
    }

    private static CSharpDataType CreateCSharpDataTypeModel(RealDataType realDataType)
    {
        return new CSharpDataType()
        {
            Name = CSharpDataTypeNames.Real,
            IsSinglePrecision = realDataType.IsSinglePrecision,
        };
    }

    private static CSharpDataType CreateCSharpDataTypeModel(DecimalDataType decimalDataType)
    {
        return new CSharpDataType()
        {
            Name = CSharpDataTypeNames.Decimal,
            Precision = decimalDataType.Precision,
            Scale = decimalDataType.Scale,
        };
    }

    private static CSharpDataType CreateCSharpDataTypeModel(StringDataType stringDataType)
    {
        return new CSharpDataType()
        {
            Name = CSharpDataTypeNames.String,
            Length = stringDataType.Length,
            IsFixedLength = stringDataType.IsFixedLength,
        };
    }

    private static CSharpDataType CreateCSharpDataTypeModel(BinaryDataType binaryDataType)
    {
        return new CSharpDataType()
        {
            Name = CSharpDataTypeNames.Binary,
            Length = binaryDataType.Length,
            IsFixedLength = binaryDataType.IsFixedLength,
        };
    }

    private static CSharpDataType CreateCSharpDataTypeModel(DateTimeDataType dateTimeDataType)
    {
        return new CSharpDataType()
        {
            Name = CSharpDataTypeNames.DateTime,
            IsWithTimeZone = dateTimeDataType.IsWithTimeZone,
        };
    }
}
