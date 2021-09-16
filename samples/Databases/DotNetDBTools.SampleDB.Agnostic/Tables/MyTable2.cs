using System;
using DotNetDBTools.Definition.Agnostic;
using DotNetDBTools.Definition.Agnostic.DbTypes;

namespace DotNetDBTools.SampleDB.Agnostic.Tables
{
    public class MyTable2 : ITable
    {
        public Guid ID => new("BFB9030C-A8C3-4882-9C42-1C6AD025CF8F");

        public Column MyColumn1 = new("C480F22F-7C01-4F41-B282-35E9F5CD1FE3")
        {
            Type = new IntDbType(),
            Nullable = false,
            Default = 333,
        };

        public Column MyColumn2 = new("5A0D1926-3270-4EB2-92EB-00BE56C7AF23")
        {
            Type = new ByteDbType() { Length = 2 },
            Nullable = true,
            Default = new byte[] { 0 },
        };

        public PrimaryKey PK_MyTable2 = new()
        {
            Columns = new string[] { nameof(MyColumn1) },
        };

        public Trigger TR_MyTable2_Name1 = new()
        {
            Code = "GetFromResurceSqlFile",
        };

        public Index IDX_MyTable2_MyIndex1 = new()
        {
            Columns = new string[] { nameof(MyColumn1), nameof(MyColumn2) },
            IncludeColumns = null,
            Unique = true,
        };
    }
}
