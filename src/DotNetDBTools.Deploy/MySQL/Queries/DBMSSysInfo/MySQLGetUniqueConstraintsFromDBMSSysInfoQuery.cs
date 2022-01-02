using System;
using DotNetDBTools.Deploy.Core.Queries.DBMSSysInfo;

namespace DotNetDBTools.Deploy.MySQL.Queries.DBMSSysInfo
{
    internal class MySQLGetUniqueConstraintsFromDBMSSysInfoQuery : GetUniqueConstraintsFromDBMSSysInfoQuery
    {
        public override string Sql => $@"SELECT NULL WHERE FALSE;";
        public override RecordMapper Mapper => throw new NotImplementedException("MySQL unique constraints are handled as indexes");
    }
}
