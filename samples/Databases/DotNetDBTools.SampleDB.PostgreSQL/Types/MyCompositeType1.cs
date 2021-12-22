using System;
using DotNetDBTools.Definition.Core;
using DotNetDBTools.Definition.PostgreSQL.UserDefinedTypes;
using DotNetDBTools.Definition.PostgreSQL.DataTypes;
using System.Collections.Generic;

namespace DotNetDBTools.SampleDB.PostgreSQL.Types
{
    public class MyCompositeType1 : ICompositeType
    {
        public Guid ID => new("29BF2520-1D74-49AB-A602-14BD692371F2");
        public IDictionary<string, IDataType> Attributes => new Dictionary<string, IDataType>()
        {
            { "MyAttribute1", new StringDataType() { Length = 100, SqlType = StringSqlType.VARCHAR } },
            { "MyAttribute2", new IntDataType() },
        };
    }
}
