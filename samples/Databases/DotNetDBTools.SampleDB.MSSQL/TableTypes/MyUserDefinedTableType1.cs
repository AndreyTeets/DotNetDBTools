using System;
using DotNetDBTools.Definition.Core.CSharpDataTypes;
using DotNetDBTools.Definition.Core.CSharpDefaultValues;
using DotNetDBTools.Definition.MSSQL;
using DotNetDBTools.Definition.MSSQL.UserDefinedTypes;

namespace DotNetDBTools.SampleDB.MSSQL.TableTypes
{
    public class MyUserDefinedTableType1 : IUserDefinedTableType
    {
        public Guid ID => new("CDBCE7BC-CAF6-4583-A72C-43AB272F7068");

        public Column MyColumn1 = new("280FB390-63B7-44A5-8E3B-9961761F20A9")
        {
            DataType = new IntDataType(),
            Default = new IntDefaultValue(777),
        };

        public CheckConstraint CK_MyUserDefinedTableType1_MyColumn1 = new("74670124-20AD-43D0-B132-AEB8B04A7FEC")
        {
            Code = $"CHECK ({nameof(MyColumn1)} >= 0)",
        };
    }
}
