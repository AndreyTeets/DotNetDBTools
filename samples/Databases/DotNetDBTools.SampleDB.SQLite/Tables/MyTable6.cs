using System;
using DotNetDBTools.Definition.Core.CSharpDataTypes;
using DotNetDBTools.Definition.SQLite;

namespace DotNetDBTools.SampleDB.SQLite.Tables
{
    public class MyTable6 : ITable
    {
        public Guid DNDBT_OBJECT_ID => new("F3064A8C-346A-4B3D-AF2C-D967B39841E4");

        public Column MyColumn1 = new("BFA08C82-5C8F-4AB4-BD41-1F1D85CF3C85")
        {
            DataType = new StringDataType() { IsFixedLength = true, Length = 4 },
        };

        public Column MyColumn2 = new("A402E2B7-C826-4CFD-A304-97C9BC346BA2")
        {
            DataType = new IntDataType(),
        };

        public ForeignKey FK_MyTable6_MyTable5_CustomName = new("AE453B22-D270-41FC-8184-9AC26B7A0569")
        {
            ThisColumns = new[] { nameof(MyColumn1), nameof(MyColumn2) },
            ReferencedTable = nameof(MyTable5),
            ReferencedTableColumns = new[] { nameof(MyTable2.MyColumn2), nameof(MyTable2.MyColumn1) },
        };
    }
}
