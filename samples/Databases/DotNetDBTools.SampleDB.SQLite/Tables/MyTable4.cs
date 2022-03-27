using System;
using DotNetDBTools.Definition.Core.CSharpDataTypes;
using DotNetDBTools.Definition.SQLite;

namespace DotNetDBTools.SampleDB.SQLite.Tables
{
    public class MyTable4 : ITable
    {
        public Guid ID => new("B12A6A37-7739-48E0-A9E1-499AE7D2A395");

        public Column MyColumn1 = new("DE0425B8-9F99-4D76-9A64-09E52F8B5D5A")
        {
            DataType = new IntDataType(),
            NotNull = true,
        };
    }
}
