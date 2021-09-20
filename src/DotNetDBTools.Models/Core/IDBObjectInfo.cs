using System;

namespace DotNetDBTools.Models.Core
{
    public interface IDBObjectInfo
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
    }
}
