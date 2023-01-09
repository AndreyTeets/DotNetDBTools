using System;
using DotNetDBTools.Definition.Core;
using DotNetDBTools.Definition.Core.CSharpDataTypes;
using DotNetDBTools.Definition.PostgreSQL;
using DotNetDBTools.SampleDB.PostgreSQL.Tables;

namespace DotNetDBTools.SampleDB.PostgreSQL.Functions
{
    public class MySequence1 : ISequence
    {
        public Guid DNDBT_OBJECT_ID => new("F54A1A93-8CD2-4125-AEDE-B38CC7F8A750");
        public IDataType DataType => new IntDataType() { Size = IntSize.Int64 };
        public SequenceOptions Options => new()
        {
            StartWith = -1000,
            IncrementBy = 2,
            MinValue = int.MinValue,
            MaxValue = 0,
            Cycle = true,
        };
        public (string TableName, string ColumnName) OwnedBy => (nameof(MyTable1), nameof(MyTable1.MyColumn1));
    }
}
