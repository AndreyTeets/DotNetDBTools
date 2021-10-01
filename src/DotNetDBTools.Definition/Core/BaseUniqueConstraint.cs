using System;
using System.Collections.Generic;

namespace DotNetDBTools.Definition.Core
{
    public abstract class BaseUniqueConstraint : IDbObject
    {
        private readonly Guid _id;
        protected BaseUniqueConstraint(string id)
        {
            _id = new Guid(id);
        }

        public Guid ID => _id;
        public IEnumerable<string> Columns { get; set; }
    }
}
