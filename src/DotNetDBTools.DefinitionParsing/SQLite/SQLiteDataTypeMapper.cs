using System;
using DotNetDBTools.CodeParsing.Core.Models;
using DotNetDBTools.Definition.Core;
using DotNetDBTools.Definition.SQLite.DataTypes;
using DotNetDBTools.DefinitionParsing.Core;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.SQLite;

namespace DotNetDBTools.DefinitionParsing.SQLite;

internal class SQLiteDataTypeMapper : IDataTypeMapper
{
    public DataType MapToDataTypeModel(IDataType dataType)
    {
        return dataType switch
        {
            IntDataType dt => new DataType() { Name = SQLiteDataTypeNames.INTEGER },
            RealDataType dt => new DataType() { Name = SQLiteDataTypeNames.REAL },
            DecimalDataType dt => new DataType() { Name = SQLiteDataTypeNames.NUMERIC },
            BoolDataType dt => new DataType() { Name = SQLiteDataTypeNames.INTEGER },

            StringDataType dt => new DataType() { Name = SQLiteDataTypeNames.TEXT },
            BinaryDataType dt => new DataType() { Name = SQLiteDataTypeNames.BLOB },
            GuidDataType dt => new DataType() { Name = SQLiteDataTypeNames.BLOB },

            DateDataType dt => new DataType() { Name = SQLiteDataTypeNames.NUMERIC },
            TimeDataType dt => new DataType() { Name = SQLiteDataTypeNames.NUMERIC },
            DateTimeDataType dt => new DataType() { Name = SQLiteDataTypeNames.NUMERIC },

            _ => throw new InvalidOperationException($"Invalid dataType: {dataType}")
        };
    }

    public DataType GetDataTypeModel(ColumnInfo column)
    {
        string normalizedDataType = column.DataType.ToUpper();
        switch (normalizedDataType)
        {
            case SQLiteDataTypeNames.INTEGER:
            case SQLiteDataTypeNames.REAL:
            case SQLiteDataTypeNames.NUMERIC:
            case SQLiteDataTypeNames.TEXT:
            case SQLiteDataTypeNames.BLOB:
                return new DataType() { Name = normalizedDataType };
            default:
                throw new InvalidOperationException($"Invalid dataType: {column.DataType}");
        };
    }
}
