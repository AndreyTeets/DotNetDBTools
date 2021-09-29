using System;
using DotNetDBTools.Definition.Agnostic.DataTypes;
using DotNetDBTools.Definition.Core;
using DotNetDBTools.DefinitionParser.Core;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.DefinitionParser.Agnostic
{
    internal class AgnosticDataTypeMapper : IDataTypeMapper
    {
        public DataTypeInfo GetDataTypeInfo(IDataType dataType)
        {
            return dataType switch
            {
                StringDataType stringDataType => GetStringDataTypeInfo(stringDataType),
                IntDataType intDataType => GetIntDataTypeInfo(intDataType),
                BinaryDataType binaryDataType => GetBinaryDataTypeInfo(binaryDataType),
                DateTimeDataType dateTimeDataType => GetDateTimeDataTypeInfo(dateTimeDataType),
                _ => throw new InvalidOperationException($"Invalid dataType: '{dataType}'")
            };
        }

        private static DataTypeInfo GetStringDataTypeInfo(StringDataType stringDataType)
        {
            return new DataTypeInfo()
            {
                Name = DataTypeNames.String,
                Length = stringDataType.Length,
                IsFixedLength = stringDataType.IsFixedLength,
            };
        }

        private static DataTypeInfo GetIntDataTypeInfo(IntDataType intDataType)
        {
            return new DataTypeInfo()
            {
                Name = DataTypeNames.Int,
                Size = int.Parse($"{intDataType.Size}".Replace("Int", "")),
            };
        }

        private static DataTypeInfo GetBinaryDataTypeInfo(BinaryDataType binaryDataType)
        {
            return new DataTypeInfo()
            {
                Name = DataTypeNames.Binary,
                Length = binaryDataType.Length,
                IsFixedLength = binaryDataType.IsFixedLength,
            };
        }

        private static DataTypeInfo GetDateTimeDataTypeInfo(DateTimeDataType _)
        {
            return new DataTypeInfo()
            {
                Name = DataTypeNames.DateTime,
            };
        }
    }
}
