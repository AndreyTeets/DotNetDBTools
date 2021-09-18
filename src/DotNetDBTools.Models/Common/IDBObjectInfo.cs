using System;

namespace DotNetDBTools.Models.Common
{
    public interface IDBObjectInfo
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
    }
}
