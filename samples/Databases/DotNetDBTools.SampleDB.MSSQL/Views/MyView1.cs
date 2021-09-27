using System;
using DotNetDBTools.Definition.MSSQL;

namespace DotNetDBTools.SampleDB.MSSQL.Views
{
    public class MyView1 : IView
    {
        public Guid ID => new("E2569AAE-D5DA-4A77-B3CD-51ADBDB272D9");
        public string Code => $"CREATE VIEW {nameof(MyView1)} bla bla";
    }
}
