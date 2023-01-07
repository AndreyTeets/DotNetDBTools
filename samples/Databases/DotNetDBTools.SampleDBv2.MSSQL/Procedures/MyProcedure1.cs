﻿using System;
using DotNetDBTools.Definition.MSSQL;

namespace DotNetDBTools.SampleDB.MSSQL.Procedures
{
    public class MyProcedure1 : IProcedure
    {
        public Guid DNDBT_OBJECT_ID => new("C4BF4926-BD3B-4C95-BC3E-1249445AEC14");
        public string CreateStatement => $"CREATE PROCEDURE {nameof(MyProcedure1)} bla bla";
    }
}
