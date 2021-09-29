using System;
using System.Collections.Generic;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Models.MSSQL;

namespace DotNetDBTools.Deploy.MSSQL.Queries.DBMSSysInfo
{
    internal class GetTypesFromDBMSSysInfoQuery : IQuery
    {
        public string Sql =>
$@"SELECT
    t.name AS {nameof(UserDefinedTypeRecord.Name)},
    t.is_nullable AS {nameof(UserDefinedTypeRecord.Nullable)},
    st.name AS {nameof(UserDefinedTypeRecord.UnderlyingDataTypeName)},
    t.max_length AS {nameof(UserDefinedTypeRecord.UnderlyingDataTypeLength)}
FROM sys.types t
INNER JOIN sys.types st
    ON st.user_type_id = t.system_type_id
WHERE t.is_user_defined = 1
    AND t.is_table_type = 0;";

        public IEnumerable<QueryParameter> Parameters => new List<QueryParameter>();

        internal class UserDefinedTypeRecord
        {
            public string Name { get; set; }
            public bool Nullable { get; set; }
            public string UnderlyingDataTypeName { get; set; }
            public string UnderlyingDataTypeLength { get; set; }
        }

        internal static class ResultsInterpreter
        {
            public static List<MSSQLUserDefinedTypeInfo> BuildUserDefinedTypesList(
                IEnumerable<UserDefinedTypeRecord> userDefinedTypeRecords)
            {
                List<MSSQLUserDefinedTypeInfo> userDefinedTypes = new();
                foreach (UserDefinedTypeRecord userDefinedTypeRecord in userDefinedTypeRecords)
                {
                    MSSQLUserDefinedTypeInfo userDefinedType = MapToMSSQLUserDefinedTypeInfo(userDefinedTypeRecord);
                    userDefinedTypes.Add(userDefinedType);
                }
                return userDefinedTypes;
            }
        }

        private static MSSQLUserDefinedTypeInfo MapToMSSQLUserDefinedTypeInfo(UserDefinedTypeRecord userDefinedTypeRecord)
        {
            return new MSSQLUserDefinedTypeInfo()
            {
                ID = Guid.NewGuid(),
                Name = userDefinedTypeRecord.Name,
                Nullable = userDefinedTypeRecord.Nullable,
                UnderlyingDataType = MSSQLSqlTypeMapper.GetModelType(
                    userDefinedTypeRecord.UnderlyingDataTypeName,
                    userDefinedTypeRecord.UnderlyingDataTypeLength),
            };
        }
    }
}
