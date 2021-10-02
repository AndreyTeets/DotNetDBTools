using System;
using DotNetDBTools.Definition.Agnostic.DataTypes;
using DotNetDBTools.Definition.Core;
using DotNetDBTools.DefinitionParsing.Core;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.DefinitionParsing.Agnostic
{
    internal class AgnosticDataTypeMapper : IDataTypeMapper
    {
        public DataType MapToDataTypeModel(IDataType dataType)
        {
            return dataType switch
            {
                StringDataType stringDataType => MapToDataTypeModel(stringDataType),
                IntDataType intDataType => MapToDataTypeModel(intDataType),
                BinaryDataType binaryDataType => MapToDataTypeModel(binaryDataType),
                DateTimeDataType dateTimeDataType => MapToDataTypeModel(dateTimeDataType),
                _ => throw new InvalidOperationException($"Invalid dataType: '{dataType}'")
            };
        }

        private static DataType MapToDataTypeModel(StringDataType stringDataType)
        {
            return new DataType()
            {
                Name = DataTypeNames.String,
                Length = stringDataType.Length,
                IsFixedLength = stringDataType.IsFixedLength,
            };
        }

        private static DataType MapToDataTypeModel(IntDataType intDataType)
        {
            return new DataType()
            {
                Name = DataTypeNames.Int,
                Size = int.Parse($"{intDataType.Size}".Replace("Int", "")),
            };
        }

        private static DataType MapToDataTypeModel(BinaryDataType binaryDataType)
        {
            return new DataType()
            {
                Name = DataTypeNames.Binary,
                Length = binaryDataType.Length,
                IsFixedLength = binaryDataType.IsFixedLength,
            };
        }

        private static DataType MapToDataTypeModel(DateTimeDataType _)
        {
            return new DataType()
            {
                Name = DataTypeNames.DateTime,
            };
        }
    }
}
