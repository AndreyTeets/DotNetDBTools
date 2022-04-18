using System;
using DotNetDBTools.Analysis.Core;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.SQLite;

namespace DotNetDBTools.Analysis.SQLite;

public class SQLiteDataTypeConverter : IDataTypeConverter
{
    public DataType Convert(CSharpDataType dataType)
    {
        return dataType.Name switch
        {
            CSharpDataTypeNames.Int => new DataType { Name = SQLiteDataTypeNames.INTEGER },
            CSharpDataTypeNames.Real => new DataType { Name = SQLiteDataTypeNames.REAL },
            CSharpDataTypeNames.Decimal => new DataType { Name = SQLiteDataTypeNames.NUMERIC },
            CSharpDataTypeNames.Bool => new DataType { Name = SQLiteDataTypeNames.INTEGER },

            CSharpDataTypeNames.String => new DataType { Name = SQLiteDataTypeNames.TEXT },
            CSharpDataTypeNames.Binary => new DataType { Name = SQLiteDataTypeNames.BLOB },
            CSharpDataTypeNames.Guid => new DataType { Name = SQLiteDataTypeNames.BLOB },

            CSharpDataTypeNames.Date => new DataType { Name = SQLiteDataTypeNames.NUMERIC },
            CSharpDataTypeNames.Time => new DataType { Name = SQLiteDataTypeNames.NUMERIC },
            CSharpDataTypeNames.DateTime => new DataType { Name = SQLiteDataTypeNames.NUMERIC },

            _ => throw new InvalidOperationException($"Invalid csharp datatype name: {dataType.Name}"),
        };
    }
}
