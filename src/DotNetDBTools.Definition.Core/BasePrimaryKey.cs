using System;
using System.Collections.Generic;

namespace DotNetDBTools.Definition.Core
{
    public abstract class BasePrimaryKey : IDbObject
    {
        public Guid ID => throw new NotImplementedException();
        public IEnumerable<string> Columns { get; set; }
    }
}
