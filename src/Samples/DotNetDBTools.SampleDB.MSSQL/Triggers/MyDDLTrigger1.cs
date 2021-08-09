using DotNetDBTools.Definition.MSSQL;
using System;

namespace DotNetDBTools.SampleDB.MSSQL.Triggers
{
    public class MyDDLTrigger1 : IDDLTrigger
    {
        public Guid ID => new("57B46518-5C2A-470D-A315-D0CB9A0FA203");
        public string Code => "GetFromResurceSqlFile";
    }
}
