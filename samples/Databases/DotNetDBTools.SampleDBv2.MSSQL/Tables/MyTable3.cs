using System;
using DotNetDBTools.Definition.Core.CSharpDataTypes;
using DotNetDBTools.Definition.Core.CSharpDefaultValues;
using DotNetDBTools.Definition.MSSQL;

namespace DotNetDBTools.SampleDB.MSSQL.Tables
{
    public class MyTable3 : ITable
    {
        public Guid DNDBT_OBJECT_ID => new("474CD761-2522-4529-9D20-2B94115F9626");

        public Column MyColumn1 = new("726F503A-D944-46EE-A0FF-6A2C2FAAB46E")
        {
            DataType = new IntDataType() { Size = IntSize.Int64 },
            NotNull = true,
            Default = new IntDefaultValue(444),
        };

        public Column MyColumn2 = new("169824E1-8B74-4B60-AF17-99656D6DBBEE")
        {
            DataType = new BinaryDataType(),
            NotNull = true,
        };

        public UniqueConstraint UQ_MyTable3_MyColumns12 = new("FD288E38-35BA-4BB1-ACE3-597C99EF26C7")
        {
            Columns = new[] { nameof(MyColumn1), nameof(MyColumn2) },
        };
    }
}
