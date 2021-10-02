using System;
using DotNetDBTools.Definition.Core;
using DotNetDBTools.Definition.SQLite.DataTypes;
using DotNetDBTools.DefinitionParsing.Core;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.DefinitionParsing.SQLite
{
    internal class SQLiteDataTypeMapper : IDataTypeMapper
    {
        public DataType MapToDataTypeModel(IDataType dataType)
        {
            return dataType switch
            {
                StringDataType stringDataType => MapToDataTypeModel(stringDataType),
                IntDataType intDataType => MapToDataTypeModel(intDataType),
                BinaryDataType binaryDataType => MapToDataTypeModel(binaryDataType),
                _ => throw new InvalidOperationException($"Invalid dataType: {dataType}")
            };
        }

        private static DataType MapToDataTypeModel(StringDataType _)
        {
            return new DataType()
            {
                Name = DataTypeNames.String,
            };
        }

        private static DataType MapToDataTypeModel(IntDataType _)
        {
            return new DataType()
            {
                Name = DataTypeNames.Int,
            };
        }

        private static DataType MapToDataTypeModel(BinaryDataType _)
        {
            return new DataType()
            {
                Name = DataTypeNames.Binary,
            };
        }
    }
}
