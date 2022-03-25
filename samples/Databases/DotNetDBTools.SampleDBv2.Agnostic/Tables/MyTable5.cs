using System;
using DotNetDBTools.Definition.Agnostic;
using DotNetDBTools.Definition.Agnostic.DataTypes;
using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.SampleDB.Agnostic.Tables
{
    public class MyTable5 : ITable
    {
        public Guid ID => new("6CA51F29-C1BC-4349-B9C1-6F1EA170F162");

        public Column MyColumn1 = new("5309D66F-2030-402E-912E-5547BABAA072")
        {
            DataType = new IntDataType(),
            NotNull = true,
            Default = new Expression("ABS(-15)"),
        };

        public Column MyColumn3 = new("4DDE852D-EC19-4B61-80F9-DA428D8FF41A")
        {
            DataType = new DateTimeDataType(),
        };
    }
}
