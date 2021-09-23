using System;

namespace DotNetDBTools.Models.Core
{
    public abstract class DBObjectInfo
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
    }
}
