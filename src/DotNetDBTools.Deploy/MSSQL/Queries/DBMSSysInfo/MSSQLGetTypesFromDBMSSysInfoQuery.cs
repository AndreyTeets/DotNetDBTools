using System;
using System.Collections.Generic;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Models.MSSQL;

namespace DotNetDBTools.Deploy.MSSQL.Queries.DBMSSysInfo
{
    internal class MSSQLGetTypesFromDBMSSysInfoQuery : IQuery
    {
        public string Sql =>
$@"SELECT
    t.name AS {nameof(UserDefinedTypeRecord.Name)},
    t.is_nullable AS {nameof(UserDefinedTypeRecord.Nullable)},
    st.name AS {nameof(UserDefinedTypeRecord.UnderlyingDataTypeName)},
    t.max_length AS {nameof(UserDefinedTypeRecord.UnderlyingDataTypeLength)},
    t.precision AS {nameof(UserDefinedTypeRecord.UnderlyingDataTypePrecision)},
    t.scale AS {nameof(UserDefinedTypeRecord.UnderlyingDataTypeScale)}
FROM sys.types t
INNER JOIN sys.types st
    ON st.user_type_id = t.system_type_id
WHERE t.is_user_defined = 1
    AND t.is_table_type = 0;";

        public IEnumerable<QueryParameter> Parameters => new List<QueryParameter>();
        public RecordMapper Mapper => new();

        public class UserDefinedTypeRecord
        {
            public string Name { get; set; }
            public bool Nullable { get; set; }
            public string UnderlyingDataTypeName { get; set; }
            public int UnderlyingDataTypeLength { get; set; }
            public int UnderlyingDataTypePrecision { get; set; }
            public int UnderlyingDataTypeScale { get; set; }
        }

        public class RecordMapper
        {
            public MSSQLUserDefinedType MapToMSSQLUserDefinedTypeModel(UserDefinedTypeRecord userDefinedTypeRecord)
            {
                return new MSSQLUserDefinedType()
                {
                    ID = Guid.NewGuid(),
                    Name = userDefinedTypeRecord.Name,
                    Nullable = userDefinedTypeRecord.Nullable,
                    UnderlyingDataType = MSSQLQueriesHelper.CreateDataTypeModel(
                        userDefinedTypeRecord.UnderlyingDataTypeName.ToUpper(),
                        userDefinedTypeRecord.UnderlyingDataTypeLength,
                        userDefinedTypeRecord.UnderlyingDataTypePrecision,
                        userDefinedTypeRecord.UnderlyingDataTypeScale),
                };
            }
        }
    }
}
