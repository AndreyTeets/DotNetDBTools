using System;

namespace DotNetDBTools.Models.Core
{
    public abstract class DBObject
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
    }
}
