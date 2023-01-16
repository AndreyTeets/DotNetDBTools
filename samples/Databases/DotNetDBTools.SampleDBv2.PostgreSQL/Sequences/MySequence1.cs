using System;
using DotNetDBTools.Definition.Core;
using DotNetDBTools.Definition.Core.CSharpDataTypes;
using DotNetDBTools.Definition.PostgreSQL;

namespace DotNetDBTools.SampleDB.PostgreSQL.Functions
{
    public class MySequence1NewName : ISequence
    {
        public Guid DNDBT_OBJECT_ID => new("F54A1A93-8CD2-4125-AEDE-B38CC7F8A750");
        public IDataType DataType => new IntDataType() { Size = IntSize.Int16 };
        public SequenceOptions Options => new()
        {
            StartWith = -1000,
            IncrementBy = 2,
            MinValue = -2000,
            MaxValue = 1500,
        };
        public (string TableName, string ColumnName) OwnedBy => (null, null);
    }
}
