using System;
using DotNetDBTools.Definition.MSSQL;
using DotNetDBTools.SampleDB.MSSQL.Tables;

namespace DotNetDBTools.SampleDB.MSSQL.Views
{
    public class MyView1 : IView
    {
        public Guid DNDBT_OBJECT_ID => new("E2569AAE-D5DA-4A77-B3CD-51ADBDB272D9");
        public string CreateStatement =>
$@"CREATE VIEW {nameof(MyView1)} AS
SELECT
    t1.{nameof(MyTable1NewName.MyColumn1)},
    t1.{nameof(MyTable1NewName.MyColumn4)},
    t2.{nameof(MyTable2.MyColumn2)}
FROM {nameof(MyTable1NewName)} t1
LEFT JOIN {nameof(MyTable2)} t2
    ON t2.{nameof(MyTable2.MyColumn1NewName)} = t1.{nameof(MyTable1NewName.MyColumn1)}";
    }
}
