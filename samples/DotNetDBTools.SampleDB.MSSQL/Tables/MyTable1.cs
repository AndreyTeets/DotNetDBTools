using System;
using DotNetDBTools.Definition.MSSQL;
using DotNetDBTools.Definition.MSSQL.DataTypes;

namespace DotNetDBTools.SampleDB.MSSQL.Tables
{
    public class MyTable1 : ITable
    {
        public Guid ID => new("DE2D4A1E-954F-4D24-80CF-D3DC75F18862");

        public Column MyColumn1 = new("CACC163C-6FDF-4030-AE11-33EBA5086E9E")
        {
            DataType = new IntDataType(),
            Default = "ABS(-15)",
            DefaultIsFunction = true,
        };

        public Column MyColumn2 = new("A9408A3C-D58E-463D-84B7-B99C53C65460")
        {
            DataType = new StringDataType() { Length = 10 },
            Default = "33",
        };

        public PrimaryKey PK_MyTable1 = new("1AFD9763-BEF9-489F-B0AF-B2C79D0AFD78")
        {
            Columns = new string[] { nameof(MyColumn1) },
        };

        public UniqueConstraint UQ_MyTable1_MyColumn2 = new("7C2E6997-AA1C-49A7-8D2A-DEC4389EBC26")
        {
            Columns = new string[] { nameof(MyColumn2) },
        };

        public ForeignKey FK_MyTable1_MyColumn1_MyTable2_MyColumn1 = new("99FA848E-D911-46E7-B406-BBD554D1C969")
        {
            ThisColumns = new string[] { nameof(MyColumn1) },
            ForeignTable = nameof(MyTable2),
            ForeignColumns = new string[] { nameof(MyTable2.MyColumn1) },
            OnUpdate = ForeignKeyActions.NoAction,
            OnDelete = ForeignKeyActions.Cascade,
        };

        public CheckConstraint CK_MyTable1_MyColumn1 = new("0A9E1C5D-A788-4F9D-B79B-65EE749957B7")
        {
            Code = $"{nameof(MyColumn1)} >= 0",
        };
    }
}
