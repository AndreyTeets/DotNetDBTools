using System;
using DotNetDBTools.Definition.MSSQL;
using DotNetDBTools.Definition.MSSQL.DataTypes;

namespace DotNetDBTools.SampleDB.MSSQL.Tables
{
    public class MyTable1 : ITable
    {
        public Guid ID => new("299675E6-4FAA-4D0F-A36A-224306BA5BCB");

        public Column MyColumn1 = new("A2F2A4DE-1337-4594-AE41-72ED4D05F317")
        {
            DataType = new IntDataType(),
            NotNull = true,
            Default = 15,
        };

        public Column MyColumn2 = new("FE68EE3D-09D0-40AC-93F9-5E441FBB4F70")
        {
            DataType = new StringDataType() { Length = 10 },
            NotNull = true,
            Default = "33",
        };

        public Column MyColumn3 = new("6E95DE30-E01A-4FB4-B8B7-8F0C40BB682C")
        {
            DataType = new IntDataType(),
            NotNull = true,
            Identity = true,
        };

        public Column MyColumn4 = new("867AC528-E87E-4C93-B6E3-DD2FCBBB837F")
        {
            DataType = new DecimalDataType(),
            NotNull = true,
            Default = 7.36m,
        };

        public PrimaryKey PK_MyTable1 = new("37A45DEF-F4A0-4BE7-8BFB-8FBED4A7D705")
        {
            Columns = new string[] { nameof(MyColumn3) },
        };

        public UniqueConstraint UQ_MyTable1_MyColumn2 = new("F3F08522-26EE-4950-9135-22EDF2E4E0CF")
        {
            Columns = new string[] { nameof(MyColumn2) },
        };

        public ForeignKey FK_MyTable1_MyColumn1_MyTable2_MyColumn1 = new("D11B2A53-32DB-432F-BB6B-F91788844BA9")
        {
            ThisColumns = new string[] { nameof(MyColumn1) },
            ReferencedTable = nameof(MyTable2),
            ReferencedTableColumns = new string[] { nameof(MyTable2.MyColumn1) },
            OnUpdate = ForeignKeyActions.NoAction,
            OnDelete = ForeignKeyActions.Cascade,
        };

        public CheckConstraint CK_MyTable1_MyCheck1 = new("EB9C59B5-BC7E-49D7-ADAA-F5600B6A19A2")
        {
            Code = $"CHECK ({nameof(MyColumn4)} >= 0)",
        };
    }
}
