using System;
using System.Collections.Generic;

namespace DotNetDBTools.Definition.Core
{
    public abstract class BaseForeignKey : IDbObject
    {
        public Guid ID => throw new NotImplementedException();
        public IEnumerable<string> ThisColumns { get; set; }
        public string ForeignTable { get; set; }
        public IEnumerable<string> ForeignColumns { get; set; }
    }
}
