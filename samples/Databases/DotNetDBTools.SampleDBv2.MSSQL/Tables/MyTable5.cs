using System;
using DotNetDBTools.Definition.MSSQL;
using DotNetDBTools.Definition.MSSQL.DataTypes;
using DotNetDBTools.SampleDB.MSSQL.Types;

namespace DotNetDBTools.SampleDB.MSSQL.Tables
{
    public class MyTable5 : ITable
    {
        public Guid ID => new("6CA51F29-C1BC-4349-B9C1-6F1EA170F162");

        public Column MyColumn1 = new("5309D66F-2030-402E-912E-5547BABAA072")
        {
            DataType = new IntDataType(),
            Default = "ABS(-15)",
            DefaultIsFunction = true,
        };

        public Column MyColumn2 = new("15AE6061-426D-4485-85E6-ECD3E0F98882")
        {
            DataType = new MyUserDefinedType1(),
            Nullable = true,
            Default = "cc",
        };
    }
}
