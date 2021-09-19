using System;
using DotNetDBTools.Definition.Agnostic;
using DotNetDBTools.Definition.Agnostic.DataTypes;

namespace DotNetDBTools.SampleDB.Agnostic.Tables
{
    public class MyTable1 : ITable
    {
        public Guid ID => new("299675E6-4FAA-4D0F-A36A-224306BA5BCB");

        public Column MyColumn1 = new("A2F2A4DE-1337-4594-AE41-72ED4D05F317")
        {
            DataType = new IntDataType(),
            Default = 15,
            Unique = true,
        };

        public Column MyColumn2 = new("FE68EE3D-09D0-40AC-93F9-5E441FBB4F70")
        {
            DataType = new StringDataType() { Length = 10 },
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
