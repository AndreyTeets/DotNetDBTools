using System;
using DotNetDBTools.Definition.SQLite;
using DotNetDBTools.Definition.SQLite.DataTypes;

namespace DotNetDBTools.SampleDB.SQLite.Tables
{
    public class MyTable1 : ITable
    {
        public Guid ID => new("BFFC40D7-6825-48F9-8442-9712C6993EF9");

        public Column MyColumn1 = new("0547CA0D-61AB-4F41-8218-DDA0C0216BEA")
        {
            DataType = new IntDataType(),
            Default = 15,
        };

        public Column MyColumn2 = new("60FF7A1F-B4B8-476F-9DB2-56617858BE35")
        {
            DataType = new StringDataType() { Length = 10 },
            Default = "33",
        };

        public PrimaryKey PK_MyTable1 = new("F8BD93B0-26EE-4564-9185-1214290622D3")
        {
            Columns = new string[] { nameof(MyColumn1) },
        };

        public UniqueConstraint UQ_MyTable1_MyColumn2 = new("08845CE3-DE5C-4B23-9DD0-91118E772A4D")
        {
            Columns = new string[] { nameof(MyColumn2) },
        };

        public ForeignKey FK_MyTable1_MyColumn1_MyTable2_MyColumn1 = new("51C1CC5E-306B-447F-804E-0943DE7B730D")
        {
            ThisColumns = new string[] { nameof(MyColumn1) },
            ForeignTable = nameof(MyTable2),
            ForeignColumns = new string[] { nameof(MyTable2.MyColumn1) },
            OnUpdate = ForeignKeyActions.NoAction,
            OnDelete = ForeignKeyActions.Cascade,
        };

        public CheckConstraint CK_MyTable1_MyColumn1 = new("4A2270DE-CF8B-4179-868D-0B169DBDD371")
        {
            Code = $"{nameof(MyColumn1)} >= 0",
        };
    }
}
