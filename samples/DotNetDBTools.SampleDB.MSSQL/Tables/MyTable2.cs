using System;
using DotNetDBTools.Definition.MSSQL;
using DotNetDBTools.Definition.MSSQL.DataTypes;
using DotNetDBTools.SampleDB.MSSQL.Types;

namespace DotNetDBTools.SampleDB.MSSQL.Tables
{
    public class MyTable2 : ITable
    {
        public Guid ID => new("5DB28241-ADAB-4ADE-87B8-75CC6CB86C60");

        public Column MyColumn1 = new("68E584A5-C69F-4B57-BB5F-4A899FCC1A74")
        {
            DataType = new IntDataType(),
            Nullable = false,
            Unique = true,
            Default = 333,
        };

        public Column MyColumn2 = new("8FB7F9E7-B460-478E-AA13-6017BCA47B25")
        {
            DataType = new MyUserDefinedType1(),
            Nullable = true,
            Default = "cc",
        };

        public PrimaryKey PK_MyTable2 = new("2938449C-3308-44E2-989D-0003EF0ECBC5")
        {
            Columns = new string[] { nameof(MyColumn1) },
        };

        public Trigger TR_MyTable2_Name1 = new("0C8BC7C1-8F7E-40EF-9753-B6974035A848")
        {
            Code = "GetFromResurceSqlFile",
        };

        public Index IDX_MyTable2_MyIndex1 = new("9D91BA60-1BB9-4EF4-BF84-FFCFDD6232D1")
        {
            Columns = new string[] { nameof(MyColumn1), nameof(MyColumn2) },
            IncludeColumns = null,
            Unique = true,
        };
    }
}
