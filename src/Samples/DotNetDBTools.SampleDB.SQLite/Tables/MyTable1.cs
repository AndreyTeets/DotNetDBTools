using DotNetDBTools.Definition.SQLite;
using DotNetDBTools.Definition.SQLite.DbTypes;
using System;

namespace DotNetDBTools.SampleDB.SQLite.Tables
{
    public class MyTable1 : ITable
    {
        public Guid ID => new("299675E6-4FAA-4D0F-A36A-224306BA5BCB");

        public Column MyColumn1 = new("0547CA0D-61AB-4F41-8218-DDA0C0216BEA")
        {
            Type = new IntDbType(),
            Default = 15,
            Unique = true,
        };

        public Column MyColumn2 = new("60FF7A1F-B4B8-476F-9DB2-56617858BE35")
        {
            Type = new StringDbType() { Length = 10 },
            Default = "33",
        };

        public PrimaryKey PK_MyTable1 = new()
        {
            Columns = new string[] { nameof(MyColumn1) },
        };

        public ForeignKey FK_MyTable1_MyColumn1_MyTable2_MyColumn1 = new()
        {
            ThisColumns = new string[] { nameof(MyColumn1) },
            ForeignTable = nameof(MyTable2),
            ForeignColumns = new string[] { nameof(MyTable2.MyColumn1) },
            OnUpdate = ForeignKeyActions.NoAction,
            OnDelete = ForeignKeyActions.Cascade,
        };

        public CheckConstraint CK_MyTable1_MyColumn1 = new()
        {
            Code = $"{nameof(MyColumn1)} >= 0",
        };
    }
}
