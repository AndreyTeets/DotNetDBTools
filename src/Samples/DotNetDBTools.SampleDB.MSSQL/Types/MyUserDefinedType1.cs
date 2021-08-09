using DotNetDBTools.Definition;
using DotNetDBTools.Definition.MSSQL;
using DotNetDBTools.Definition.MSSQL.DbTypes;
using System;

namespace DotNetDBTools.SampleDB.MSSQL.Types
{
    public class MyUserDefinedType1 : IUserDefinedType
    {
        public Guid ID => new("0CD1E71C-CC9C-440F-AC0B-81A1D6F7DDAA");
        public IDbType UnderlyingType => new StringDbType() { Length = 100 };
        public bool Nullable => true;
    }
}
