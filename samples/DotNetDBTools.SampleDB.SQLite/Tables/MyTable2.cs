using System;
using DotNetDBTools.Definition.SQLite;
using DotNetDBTools.Definition.SQLite.DbTypes;

namespace DotNetDBTools.SampleDB.SQLite.Tables
{
    public class MyTable2 : ITable
    {
        public Guid ID => new("562EC55B-6C11-4DDE-B445-F062B12CA4AC");

        public Column MyColumn1 = new("1DB86894-78F0-4BC4-97CF-FC1AA5321E77")
        {
            Type = new IntDbType(),
            Nullable = false,
            Default = 333,
        };

        public Column MyColumn2 = new("28C62B20-40E5-463C-973D-A40F25353E63")
        {
            Type = new ByteDbType(),
            Nullable = true,
            Default = new byte[] { 0, 1 },
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
