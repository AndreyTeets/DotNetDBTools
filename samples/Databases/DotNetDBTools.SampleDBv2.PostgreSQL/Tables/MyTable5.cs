using System;
using DotNetDBTools.Definition.PostgreSQL;
using DotNetDBTools.Definition.PostgreSQL.DataTypes;
using DotNetDBTools.SampleDB.PostgreSQL.Types;

namespace DotNetDBTools.SampleDB.PostgreSQL.Tables
{
    public class MyTable5 : ITable
    {
        public Guid ID => new("6CA51F29-C1BC-4349-B9C1-6F1EA170F162");

        public Column MyColumn1 = new("5309D66F-2030-402E-912E-5547BABAA072")
        {
            DataType = new IntDataType(),
            NotNull = true,
            Default = @"""MyFunction1""(-25, 10)",
            DefaultIsFunction = true,
        };

        public Column MyColumn2 = new("15AE6061-426D-4485-85E6-ECD3E0F98882")
        {
            DataType = new MyCompositeType1(),
            NotNull = true,
        };

        public Column MyColumn3 = new("4DDE852D-EC19-4B61-80F9-DA428D8FF41A")
        {
            DataType = new DateTimeDataType() { IsWithTimeZone = true },
        };

        public Column MyColumn4 = new("45856161-DB66-49F6-AFDE-9214D2D2D4B0")
        {
            DataType = new MyDomain1(),
            NotNull = true,
        };

        public Column MyColumn5 = new("B45D163B-F49E-499F-A9E5-2538CD073B80")
        {
            DataType = new MyEnumType1(),
            NotNull = true,
        };

        public Column MyColumn6 = new("C8B03B75-A8A2-47E0-BF5C-F3E4F1B8F500")
        {
            DataType = new MyRangeType1(),
            NotNull = true,
        };
    }
}
