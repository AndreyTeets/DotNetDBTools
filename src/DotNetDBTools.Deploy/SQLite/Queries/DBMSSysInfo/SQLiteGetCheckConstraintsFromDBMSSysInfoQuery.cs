using System;
using DotNetDBTools.Deploy.Core.Queries.DBMSSysInfo;

namespace DotNetDBTools.Deploy.SQLite.Queries.DBMSSysInfo
{
    internal class SQLiteGetCheckConstraintsFromDBMSSysInfoQuery : GetCheckConstraintsFromDBMSSysInfoQuery
    {
        public override string Sql => $@"SELECT NULL WHERE FALSE;";
        public override RecordMapper Mapper => throw new NotImplementedException("SQLite check constraints are parsed from table definitions");
    }
}
