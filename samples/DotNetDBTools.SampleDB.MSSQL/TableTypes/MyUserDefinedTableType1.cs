using System;
using DotNetDBTools.Definition.MSSQL;
using DotNetDBTools.Definition.MSSQL.DbTypes;

namespace DotNetDBTools.SampleDB.MSSQL.TableTypes
{
    public class MyUserDefinedTableType1 : IUserDefinedTableType
    {
        public Guid ID => new("CDBCE7BC-CAF6-4583-A72C-43AB272F7068");

        public Column MyColumn1 = new("280FB390-63B7-44A5-8E3B-9961761F20A9")
        {
            Type = new IntDbType(),
            Nullable = true,
            Unique = false,
            Identity = false,
            Default = 666,
            ComputeCode = null,
        };

        public CheckConstraint CK_MyUserDefinedTableType1_MyColumn1 = new()
        {
            Code = $"{nameof(MyColumn1)} >= 0",
        };
    }
}
