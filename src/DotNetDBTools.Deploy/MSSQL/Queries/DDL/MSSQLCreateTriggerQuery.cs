using DotNetDBTools.Deploy.Common.Queries.DDL;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.MSSQL.Queries.DDL
{
    internal class MSSQLCreateTriggerQuery : CreateTriggerQuery
    {
        public MSSQLCreateTriggerQuery(Trigger trigger)
            : base(trigger) { }

        protected override string GetSql(Trigger trigger)
        {
            string query =
$@"{trigger.GetCode()}";

            return query;
        }
    }
}
