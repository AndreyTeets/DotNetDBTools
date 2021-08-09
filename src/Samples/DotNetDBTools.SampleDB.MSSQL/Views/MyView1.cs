using DotNetDBTools.Definition.MSSQL;
using System;

namespace DotNetDBTools.SampleDB.MSSQL.Views
{
    public class MyView1 : IView
    {
        public Guid ID => new("03469828-1058-41DF-B093-593322459B2A");
        public string Code => "GetFromResurceSqlFile";
    }
}
