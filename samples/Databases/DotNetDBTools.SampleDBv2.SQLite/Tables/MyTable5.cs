using System;
using DotNetDBTools.Definition.Core.CSharpDataTypes;
using DotNetDBTools.Definition.Core.CSharpDefaultValues;
using DotNetDBTools.Definition.SQLite;

namespace DotNetDBTools.SampleDB.SQLite.Tables
{
    public class MyTable5 : ITable
    {
        public Guid DNDBT_OBJECT_ID => new("6CA51F29-C1BC-4349-B9C1-6F1EA170F162");

        public Column MyColumn1 = new("5309D66F-2030-402E-912E-5547BABAA072")
        {
            DataType = new VerbatimDataType("INTEGER"),
            NotNull = true,
            Default = new VerbatimDefaultValue("(ABS(-15))"),
        };

        public Column MyColumn2 = new("11EF8E25-3691-42D4-B2FA-88D724F73B61")
        {
            DataType = new StringDataType() { IsFixedLength = true, Length = 4 },
            NotNull = true,
            Default = new StringDefaultValue("test"),
        };

        public Column MyColumn3 = new("6ED0AB37-AAD3-4294-9BA6-C0921F0E67AF")
        {
            DataType = new BinaryDataType() { IsFixedLength = true, Length = 3 },
            NotNull = true,
            Default = new BinaryDefaultValue("0x000204"),
        };

        public Column MyColumn4 = new("ACA57FD6-80D0-4C18-B2CA-AABCB06BEA10")
        {
            DataType = new RealDataType() { IsSinglePrecision = true },
            NotNull = true,
            Default = new RealDefaultValue(123.456d),
        };

        public Column MyColumn5 = new("47666B8B-CA72-4507-86B2-04C47A84AED4")
        {
            DataType = new RealDataType(),
            NotNull = true,
            Default = new RealDefaultValue("12345.6789"),
        };

        public Column MyColumn6 = new("98FDED6C-D486-4A2E-9C9A-1EC31C9D5830")
        {
            DataType = new DecimalDataType() { Precision = 6, Scale = 1 },
            NotNull = true,
            Default = new DecimalDefaultValue(12.3m),
        };

        public Column MyColumn7 = new("2502CADE-458A-48EE-9421-E6D7850493F7")
        {
            DataType = new BoolDataType(),
            NotNull = true,
            Default = new BoolDefaultValue(true),
        };

        public Column MyColumn8 = new("ED044A8A-6858-41E2-A867-9E5B01F226C8")
        {
            DataType = new GuidDataType(),
            NotNull = true,
            Default = new GuidDefaultValue("8E2F99AD-0FC8-456D-B0E4-EC3BA572DD15"),
        };

        public Column MyColumn9 = new("9939D676-73B7-42D1-BA3E-5C13AED5CE34")
        {
            DataType = new DateDataType(),
            NotNull = true,
            Default = new DateDefaultValue("2022-02-15"),
        };

        public Column MyColumn10 = new("CBA4849B-3D84-4E38-B2C8-F9DBDFF22FA6")
        {
            DataType = new TimeDataType(),
            NotNull = true,
            Default = new TimeDefaultValue("16:17:18"),
        };

        public Column MyColumn11 = new("4DDE852D-EC19-4B61-80F9-DA428D8FF41A")
        {
            DataType = new DateTimeDataType(),
            NotNull = true,
            Default = new DateTimeDefaultValue("2022-02-15 16:17:18"),
        };

        public Column MyColumn12 = new("685FAF2E-FEF7-4E6B-A960-ACD093F1F004")
        {
            DataType = new DateTimeDataType() { IsWithTimeZone = true },
            NotNull = true,
            Default = new DateTimeDefaultValue("2022-02-15 16:17:18+01:30"),
        };
    }
}
