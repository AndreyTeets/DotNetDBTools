using System;

namespace DotNetDBTools.Models.Shared
{
    public interface IDBObjectInfo
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
    }
}
