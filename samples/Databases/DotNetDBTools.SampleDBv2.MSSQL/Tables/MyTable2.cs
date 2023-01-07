using System;
using DotNetDBTools.Definition.Core.CSharpDataTypes;
using DotNetDBTools.Definition.Core.CSharpDefaultValues;
using DotNetDBTools.Definition.MSSQL;

namespace DotNetDBTools.SampleDB.MSSQL.Tables
{
    public class MyTable2 : ITable
    {
        public Guid DNDBT_OBJECT_ID => new("BFB9030C-A8C3-4882-9C42-1C6AD025CF8F");

        public Column MyColumn1NewName = new("C480F22F-7C01-4F41-B282-35E9F5CD1FE3")
        {
            DataType = new IntDataType() { Size = IntSize.Int64 },
            NotNull = true,
            Default = new IntDefaultValue(333),
        };

        public Column MyColumn2 = new("C2DF19C2-E029-4014-8A5B-4AB42FECB6B8")
        {
            DataType = new BinaryDataType() { Length = 22 },
            Default = new BinaryDefaultValue(new byte[] { 0, 4, 8 }),
        };

        public Column MyColumn3 = new("99BC3F49-3151-4F52-87F7-104B424ED7BF")
        {
            DataType = new IntDataType() { Size = IntSize.Int64 },
        };

        public Column MyColumn4 = new("87950A3F-2072-42DB-AC3C-A4E85B79720D")
        {
            DataType = new BinaryDataType(),
        };

        public PrimaryKey PK_MyTable2_CustomName = new("3A43615B-40B3-4A13-99E7-93AF7C56E8CE")
        {
            Columns = new[] { nameof(MyColumn1NewName) },
        };

        public ForeignKey FK_MyTable2_MyColumns34_MyTable3_MyColumns12 = new("480F3508-9D51-4190-88AA-45BC20E49119")
        {
            ThisColumns = new[] { nameof(MyColumn3), nameof(MyColumn4) },
            ReferencedTable = nameof(MyTable3),
            ReferencedTableColumns = new[] { nameof(MyTable3.MyColumn1), nameof(MyTable3.MyColumn2) },
            OnUpdate = ForeignKeyActions.NoAction,
            OnDelete = ForeignKeyActions.SetDefault,
        };

        public Trigger TR_MyTable2_MyTrigger1 = new("EE64FFC3-5536-4624-BEAF-BC3A61D06A1A")
        {
            CreateStatement = $"Triggers.{nameof(TR_MyTable2_MyTrigger1)}.sql".AsSqlResource(),
        };

        public Index IDX_MyTable2_MyIndex1 = new("74390B3C-BC39-4860-A42E-12BAA400F927")
        {
            Columns = new[] { nameof(MyColumn1NewName), nameof(MyColumn2) },
            IncludeColumns = null,
            Unique = true,
        };
    }
}
