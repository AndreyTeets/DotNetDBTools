using System;
using DotNetDBTools.Definition.SQLite;
using DotNetDBTools.Definition.SQLite.DataTypes;

namespace DotNetDBTools.SampleDB.SQLite.Tables
{
    public class MyTable2 : ITable
    {
        public Guid ID => new("BFB9030C-A8C3-4882-9C42-1C6AD025CF8F");

        public Column MyColumn1NewName = new("C480F22F-7C01-4F41-B282-35E9F5CD1FE3")
        {
            DataType = new IntDataType(),
            NotNull = true,
            Default = 333,
        };

        public Column MyColumn2 = new("C2DF19C2-E029-4014-8A5B-4AB42FECB6B8")
        {
            DataType = new BinaryDataType(),
            Default = new byte[] { 0, 1, 2 },
        };

        public PrimaryKey PK_MyTable2 = new("3A43615B-40B3-4A13-99E7-93AF7C56E8CE")
        {
            Columns = new string[] { nameof(MyColumn1NewName) },
        };

        public ForeignKey FK_MyTable2_MyColumns12_MyTable3_MyColumns12 = new("480F3508-9D51-4190-88AA-45BC20E49119")
        {
            ThisColumns = new string[] { nameof(MyColumn1NewName), nameof(MyColumn2) },
            ReferencedTable = nameof(MyTable3),
            ReferencedTableColumns = new string[] { nameof(MyTable3.MyColumn1), nameof(MyTable3.MyColumn2) },
            OnUpdate = ForeignKeyActions.NoAction,
            OnDelete = ForeignKeyActions.SetDefault,
        };

        public Trigger TR_MyTable2_MyTrigger1 = new("EE64FFC3-5536-4624-BEAF-BC3A61D06A1A")
        {
            Code = $"Triggers.{nameof(TR_MyTable2_MyTrigger1)}.sql".AsSqlResource(),
        };

        public Index IDX_MyTable2_MyIndex1 = new("74390B3C-BC39-4860-A42E-12BAA400F927")
        {
            Columns = new string[] { nameof(MyColumn1NewName), nameof(MyColumn2) },
            IncludeColumns = null,
            Unique = true,
        };
    }
}
