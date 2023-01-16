using System;
using DotNetDBTools.Definition.Core;
using DotNetDBTools.Definition.Core.CSharpDataTypes;
using DotNetDBTools.Definition.PostgreSQL;
using DotNetDBTools.SampleDB.PostgreSQL.Tables;

namespace DotNetDBTools.SampleDB.PostgreSQL.Functions
{
    public class MySequence2 : ISequence
    {
        public Guid DNDBT_OBJECT_ID => new("59C3BF9D-4938-40DF-9528-F1AA8367C6E3");
        public IDataType DataType => new IntDataType();
        public SequenceOptions Options => new();
        public (string TableName, string ColumnName) OwnedBy => (nameof(MyTable1), nameof(MyTable1.MyColumn2));
    }
}
