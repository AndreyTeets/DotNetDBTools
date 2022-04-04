using System;
using DotNetDBTools.Definition.Core.CSharpDataTypes;
using DotNetDBTools.Definition.Core.CSharpDefaultValues;
using DotNetDBTools.Definition.MySQL;

namespace DotNetDBTools.SampleDB.MySQL.Tables
{
    public class MyTable2 : ITable
    {
        public Guid ID => new("BFB9030C-A8C3-4882-9C42-1C6AD025CF8F");

        public Column MyColumn1 = new("C480F22F-7C01-4F41-B282-35E9F5CD1FE3")
        {
            DataType = new IntDataType(),
            NotNull = true,
            Default = new IntDefaultValue(333),
        };

        public Column MyColumn2 = new("5A0D1926-3270-4EB2-92EB-00BE56C7AF23")
        {
            DataType = new BinaryDataType() { Length = 22 },
            Default = new BinaryDefaultValue(new byte[] { 0, 1, 2 }),
        };

        public PrimaryKey PK_MyTable2_CustomName = new("3A43615B-40B3-4A13-99E7-93AF7C56E8CE")
        {
            Columns = new string[] { nameof(MyColumn1) },
        };

        public Trigger TR_MyTable2_MyTrigger1 = new("EE64FFC3-5536-4624-BEAF-BC3A61D06A1A")
        {
            Code = $"Triggers.{nameof(TR_MyTable2_MyTrigger1)}.sql".AsSqlResource(),
        };

        public Index IDX_MyTable2_MyIndex1 = new("74390B3C-BC39-4860-A42E-12BAA400F927")
        {
            Columns = new string[] { nameof(MyColumn1), nameof(MyColumn2) },
            Unique = true,
        };
    }
}
