using System;
using DotNetDBTools.Definition.PostgreSQL;
using DotNetDBTools.SampleDB.PostgreSQL.Tables;

namespace DotNetDBTools.SampleDB.PostgreSQL.Views
{
    public class MyView1 : IView
    {
        public Guid ID => new("E2569AAE-D5DA-4A77-B3CD-51ADBDB272D9");
        public string Code =>
$@"CREATE VIEW ""{nameof(MyView1)}"" AS
SELECT
    t1.""{nameof(MyTable1NewName.MyColumn1)}"",
    t1.""{nameof(MyTable1NewName.MyColumn4)}"",
    t2.""{nameof(MyTable2.MyColumn2)}""
FROM ""{nameof(MyTable1NewName)}"" t1
LEFT JOIN ""{nameof(MyTable2)}"" t2
    ON t2.""{nameof(MyTable2.MyColumn1NewName)}"" = t1.""{nameof(MyTable1NewName.MyColumn1)}""";
    }
}
