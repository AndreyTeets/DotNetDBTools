using System;
using System.Collections.Generic;

namespace DotNetDBTools.Models.Core
{
    public abstract class DBObject
    {
        public Guid ID { get; set; }
        public string Name { get; set; }

        public DBObject Parent { get; set; }

        public List<DBObject> DependsOn { get; set; }
        public List<DBObject> IsDependencyOf { get; set; }
    }
}
