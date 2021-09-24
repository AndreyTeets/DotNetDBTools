using System;
using DotNetDBTools.Definition.MSSQL;
using DotNetDBTools.Definition.MSSQL.DataTypes;

namespace DotNetDBTools.SampleDB.MSSQL.Tables
{
    public class MyTable1NewName : ITable
    {
        public Guid ID => new("299675E6-4FAA-4D0F-A36A-224306BA5BCB");

        public Column MyColumn1 = new("A2F2A4DE-1337-4594-AE41-72ED4D05F317")
        {
            DataType = new IntDataType(),
            Nullable = true,
            Default = 15,
        };

        public ForeignKey FK_MyTable1_MyColumn1_MyTable2_MyColumn1 = new("D11B2A53-32DB-432F-BB6B-F91788844BA9")
        {
            ThisColumns = new string[] { nameof(MyColumn1) },
            ForeignTable = nameof(MyTable2),
            ForeignColumns = new string[] { nameof(MyTable2.MyColumn1NewName) },
            OnUpdate = ForeignKeyActions.NoAction,
            OnDelete = ForeignKeyActions.SetNull,
        };

        public CheckConstraint CK_MyTable1_MyColumn1 = new("EB9C59B5-BC7E-49D7-ADAA-F5600B6A19A2")
        {
            Code = $"{nameof(MyColumn1)} >= 0",
        };
    }
}
