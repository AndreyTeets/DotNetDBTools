using System;
using DotNetDBTools.Definition.MySQL;

namespace DotNetDBTools.SampleDB.MySQL.Procedures
{
    public class MyProcedure1 : IProcedure
    {
        public Guid ID => new("C4BF4926-BD3B-4C95-BC3E-1249445AEC14");
        public string Code => $"CREATE PROCEDURE {nameof(MyProcedure1)} bla bla";
    }
}
