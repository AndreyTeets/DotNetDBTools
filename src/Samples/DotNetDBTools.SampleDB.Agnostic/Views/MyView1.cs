using DotNetDBTools.Definition.Agnostic;
using System;

namespace DotNetDBTools.SampleDB.Agnostic.Views
{
    public class MyView1 : IView
    {
        public Guid ID => new("E2569AAE-D5DA-4A77-B3CD-51ADBDB272D9");
        public string Code => "GetFromResurceSqlFile";
    }
}
