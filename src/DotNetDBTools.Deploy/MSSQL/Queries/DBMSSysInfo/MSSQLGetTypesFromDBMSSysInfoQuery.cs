﻿using System;
using DotNetDBTools.Deploy.Core.Queries;
using DotNetDBTools.Models.MSSQL.UserDefinedTypes;

namespace DotNetDBTools.Deploy.MSSQL.Queries.DBMSSysInfo;

internal class MSSQLGetTypesFromDBMSSysInfoQuery : NoParametersQuery
{
    public override string Sql =>
$@"SELECT
    t.name AS [{nameof(UserDefinedTypeRecord.Name)}],
    st.name AS [{nameof(UserDefinedTypeRecord.UnderlyingTypeName)}],
    ~t.is_nullable AS [{nameof(UserDefinedTypeRecord.NotNull)}],
    t.max_length AS [{nameof(UserDefinedTypeRecord.UnderlyingTypeLength)}],
    t.precision AS [{nameof(UserDefinedTypeRecord.UnderlyingTypePrecision)}],
    t.scale AS [{nameof(UserDefinedTypeRecord.UnderlyingTypeScale)}]
FROM sys.types t
INNER JOIN sys.types st
    ON st.user_type_id = t.system_type_id
WHERE t.is_user_defined = 1
    AND t.is_table_type = 0;";

    public RecordMapper Mapper => new();

    public class UserDefinedTypeRecord
    {
        public string Name { get; set; }
        public bool NotNull { get; set; }
        public string UnderlyingTypeName { get; set; }
        public int UnderlyingTypeLength { get; set; }
        public int UnderlyingTypePrecision { get; set; }
        public int UnderlyingTypeScale { get; set; }
    }

    public class RecordMapper
    {
        public MSSQLUserDefinedType MapToMSSQLUserDefinedTypeModel(UserDefinedTypeRecord userDefinedTypeRecord)
        {
            return new MSSQLUserDefinedType()
            {
                ID = Guid.NewGuid(),
                Name = userDefinedTypeRecord.Name,
                NotNull = userDefinedTypeRecord.NotNull,
                UnderlyingType = MSSQLQueriesHelper.CreateDataTypeModel(
                    userDefinedTypeRecord.UnderlyingTypeName.ToUpper(),
                    userDefinedTypeRecord.UnderlyingTypeLength,
                    userDefinedTypeRecord.UnderlyingTypePrecision,
                    userDefinedTypeRecord.UnderlyingTypeScale),
            };
        }
    }
}
