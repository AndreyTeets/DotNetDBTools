using System;

namespace DotNetDBTools.Models.Core
{
    public abstract class BaseDBObjectInfo : IDBObjectInfo
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
    }
}
