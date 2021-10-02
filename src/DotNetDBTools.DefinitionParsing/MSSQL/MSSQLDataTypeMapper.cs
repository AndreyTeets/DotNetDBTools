using System;
using DotNetDBTools.Definition.Core;
using DotNetDBTools.Definition.MSSQL;
using DotNetDBTools.Definition.MSSQL.DataTypes;
using DotNetDBTools.DefinitionParsing.Core;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.DefinitionParsing.MSSQL
{
    internal class MSSQLDataTypeMapper : IDataTypeMapper
    {
        public DataType MapToDataTypeModel(IDataType dataType)
        {
            return dataType switch
            {
                StringDataType stringDataType => MapToDataTypeModel(stringDataType),
                IntDataType intDataType => MapToDataTypeModel(intDataType),
                BinaryDataType binaryDataType => MapToDataTypeModel(binaryDataType),
                DateTimeDataType dateTimeDataType => MapToDataTypeModel(dateTimeDataType),
                IUserDefinedType userDefinedType => MapToDataTypeModel(userDefinedType),
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
                IsUnicode = stringDataType.IsUnicode,
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

        private static DataType MapToDataTypeModel(DateTimeDataType dateTimeDataType)
        {
            return new DataType()
            {
                Name = DataTypeNames.DateTime,
                SqlTypeName = $"{dateTimeDataType.MSSQLType}",
            };
        }

        private DataType MapToDataTypeModel(IUserDefinedType userDefinedType)
        {
            DataType dataTypeModel = MapToDataTypeModel(userDefinedType.UnderlyingType);
            dataTypeModel.Name = userDefinedType.GetType().Name;
            dataTypeModel.IsUserDefined = true;
            return dataTypeModel;
        }
    }
}
