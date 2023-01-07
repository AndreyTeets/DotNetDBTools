﻿using System;
using DotNetDBTools.Definition.MySQL;

namespace DotNetDBTools.SampleDB.MySQL.Functions
{
    public class MyFunction1 : IFunction
    {
        public Guid DNDBT_OBJECT_ID => new("63D3A414-2893-4462-B3F8-04633101263A");
        public string CreateStatement => $"CREATE FUNCTION {nameof(MyFunction1)} bla bla";
    }
}
