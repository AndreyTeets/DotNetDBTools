using System;
using DotNetDBTools.Definition.Core.CSharpDataTypes;
using DotNetDBTools.Definition.PostgreSQL;

namespace DotNetDBTools.SampleDB.PostgreSQL.Tables
{
    public class MyTable4 : ITable
    {
        public Guid DNDBT_OBJECT_ID => new("B12A6A37-7739-48E0-A9E1-499AE7D2A395");

        public Column MyColumn1 = new("DE0425B8-9F99-4D76-9A64-09E52F8B5D5A")
        {
            DataType = new IntDataType() { Size = IntSize.Int64 },
            NotNull = true,
        };

        public Column MyColumn2 = new("A6354EA4-7113-4C14-8047-648F0CFC7163")
        {
            DataType = new VerbatimDataType("inT"),
            NotNull = true,
            Identity = true,
        };

        public PrimaryKey PK_MyTable4 = new("53AD5415-7FEA-4A51-BCAE-65E349A2E477")
        {
            Columns = new[] { nameof(MyColumn2) },
        };
    }
}
