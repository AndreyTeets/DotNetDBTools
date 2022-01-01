using System;
using DotNetDBTools.Definition.Agnostic;
using DotNetDBTools.SampleDB.Agnostic.Tables;

namespace DotNetDBTools.SampleDB.Agnostic.Views
{
    public class MyView1 : IView
    {
        public Guid ID => new("E2569AAE-D5DA-4A77-B3CD-51ADBDB272D9");
        public Func<DbmsKind, string> Code => dk =>
@$"CREATE VIEW {nameof(MyView1).Quote(dk)} AS
SELECT
    t1.{nameof(MyTable1.MyColumn1).Quote(dk)},
    t1.{nameof(MyTable1.MyColumn4).Quote(dk)},
    t2.{nameof(MyTable2.MyColumn2).Quote(dk)}
FROM {nameof(MyTable1).Quote(dk)} t1
LEFT JOIN {nameof(MyTable2).Quote(dk)} t2
    ON t2.{nameof(MyTable2.MyColumn1).Quote(dk)} = t1.{nameof(MyTable1.MyColumn1).Quote(dk)};";
    }
}
