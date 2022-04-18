using System.Collections.Generic;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.MSSQL.Queries.DBMSSysInfo;
using DotNetDBTools.Deploy.MSSQL.Queries.DNDBTSysInfo;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MSSQL;
using DotNetDBTools.Models.MSSQL.UserDefinedTypes;
using static DotNetDBTools.Deploy.Core.Queries.DNDBTSysInfo.GetDNDBTDbObjectRecordsQuery;
using static DotNetDBTools.Deploy.MSSQL.Queries.DBMSSysInfo.MSSQLGetTypesFromDBMSSysInfoQuery;

namespace DotNetDBTools.Deploy.MSSQL;

internal class MSSQLDbModelFromDBMSProvider : DbModelFromDBMSProvider<
    MSSQLDatabase,
    MSSQLTable,
    MSSQLView,
    MSSQLGetColumnsFromDBMSSysInfoQuery,
    MSSQLGetPrimaryKeysFromDBMSSysInfoQuery,
    MSSQLGetUniqueConstraintsFromDBMSSysInfoQuery,
    MSSQLGetCheckConstraintsFromDBMSSysInfoQuery,
    MSSQLGetIndexesFromDBMSSysInfoQuery,
    MSSQLGetTriggersFromDBMSSysInfoQuery,
    MSSQLGetForeignKeysFromDBMSSysInfoQuery,
    MSSQLGetViewsFromDBMSSysInfoQuery,
    MSSQLGetDNDBTDbAttributesRecordQuery,
    MSSQLGetDNDBTDbObjectRecordsQuery,
    MSSQLGetDNDBTScriptExecutionRecordsQuery>
{
    public MSSQLDbModelFromDBMSProvider(IQueryExecutor queryExecutor)
        : base(queryExecutor) { }

    protected override void ReplaceAdditionalDbModelObjectsIDsAndCodeWithDNDBTSysInfo(Database database, Dictionary<string, DNDBTInfo> dbObjectIDsMap)
    {
        MSSQLDatabase db = (MSSQLDatabase)database;
        foreach (MSSQLUserDefinedType udt in db.UserDefinedTypes)
            udt.ID = dbObjectIDsMap[$"{DbObjectType.UserDefinedType}_{udt.Name}_{null}"].ID;
    }

    protected override void BuildAdditionalDbObjects(Database database)
    {
        MSSQLDatabase db = (MSSQLDatabase)database;
        db.UserDefinedTypes = BuildUserDefinedTypes(new MSSQLGetTypesFromDBMSSysInfoQuery());
        db.UserDefinedTableTypes = new();
        db.Functions = new();
        db.Procedures = new();
    }

    private List<MSSQLUserDefinedType> BuildUserDefinedTypes(MSSQLGetTypesFromDBMSSysInfoQuery query)
    {
        IEnumerable<UserDefinedTypeRecord> userDefinedTypeRecords = QueryExecutor.Query<UserDefinedTypeRecord>(query);
        List<MSSQLUserDefinedType> userDefinedTypes = new();
        foreach (UserDefinedTypeRecord userDefinedTypeRecord in userDefinedTypeRecords)
        {
            MSSQLUserDefinedType userDefinedType = query.Mapper.MapToMSSQLUserDefinedTypeModel(userDefinedTypeRecord);
            userDefinedTypes.Add(userDefinedType);
        }
        return userDefinedTypes;
    }
}
