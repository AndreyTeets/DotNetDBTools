using System;
using System.Collections.Generic;

namespace DotNetDBTools.Definition.Core
{
    public abstract class BaseForeignKey : IDbObject
    {
        private readonly Guid _id;
        protected BaseForeignKey(string id)
        {
            _id = new Guid(id);
        }

        public Guid ID => _id;
        public IEnumerable<string> ThisColumns { get; set; }
        public string ForeignTable { get; set; }
        public IEnumerable<string> ForeignColumns { get; set; }
    }
}
