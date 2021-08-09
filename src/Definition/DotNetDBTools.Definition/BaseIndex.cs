using System;
using System.Collections.Generic;

namespace DotNetDBTools.Definition
{
    public abstract class BaseIndex : IDbObject
    {
        public Guid ID => throw new NotImplementedException();
        public IEnumerable<string> Columns { get; set; }
        public IEnumerable<string> IncludeColumns { get; set; }
        public bool Unique { get; set; }
    }
}
