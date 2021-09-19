using System;

namespace DotNetDBTools.Models.Shared
{
    public abstract class BaseDBObjectInfo : IDBObjectInfo
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
    }
}
