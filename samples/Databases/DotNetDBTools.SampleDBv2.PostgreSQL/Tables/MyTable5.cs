using System;
using DotNetDBTools.Definition.Core.CSharpDataTypes;
using DotNetDBTools.Definition.Core.CSharpDefaultValues;
using DotNetDBTools.Definition.PostgreSQL;
using DotNetDBTools.SampleDB.PostgreSQL.Functions;
using DotNetDBTools.SampleDB.PostgreSQL.Types;

namespace DotNetDBTools.SampleDB.PostgreSQL.Tables
{
    public class MyTable5 : ITable
    {
        public Guid DNDBT_OBJECT_ID => new("6CA51F29-C1BC-4349-B9C1-6F1EA170F162");

        public Column MyColumn1 = new("5309D66F-2030-402E-912E-5547BABAA072")
        {
            DataType = new VerbatimDataType("int"),
            NotNull = true,
            Default = new VerbatimDefaultValue("abS(-15)"),
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

        public Column MyColumn101 = new("15AE6061-426D-4485-85E6-ECD3E0F98882")
        {
            DataType = new MyCompositeType1(),
            NotNull = true,
        };

        public Column MyColumn102 = new("45856161-DB66-49F6-AFDE-9214D2D2D4B0")
        {
            DataType = new MyDomain1(),
            NotNull = true,
        };

        public Column MyColumn103 = new("B45D163B-F49E-499F-A9E5-2538CD073B80")
        {
            DataType = new MyEnumType1(),
            NotNull = true,
        };

        public Column MyColumn104 = new("C8B03B75-A8A2-47E0-BF5C-F3E4F1B8F500")
        {
            DataType = new MyRangeType1(),
            NotNull = true,
        };

        public Column MyColumn201 = new("5C455EC9-9830-4D0B-A88C-57341899DC4A")
        {
            DataType = new VerbatimDataType("INT"),
            Default = new VerbatimDefaultValue($"{nameof(MyFunction1).Quote()}(-25, 10)"),
        };

        public Column MyColumn202 = new("029A13BC-D972-45C8-8A6E-6E1ACC3F25B1")
        {
            DataType = new VerbatimDataType($"{nameof(MyDomain2).Quote()}"),
            NotNull = true,
        };

        public Column MyColumn203 = new("C1F037D5-0656-43D1-8F30-F0B7B452D594")
        {
            DataType = new IntDataType(),
            NotNull = true,
            Identity = true,
            IdentityGenerationKind = IdentityGenerationKind.ByDefault,
            IdentitySequenceOptions = new()
            {
                IncrementBy = -4,
                MinValue = int.MinValue,
                MaxValue = 9999,
                Cycle = true,
            },
        };

        public Column MyColumn301 = new("5A6FDF6E-F39E-41BF-84FD-6B1BECAB248B")
        {
            DataType = new IntDataType() { Size = IntSize.Int8 },
        };

        public Column MyColumn302 = new("4F4B70FD-178D-468F-8575-1D41ED28AFC4")
        {
            DataType = new IntDataType() { Size = IntSize.Int16 },
        };

        public Column MyColumn303 = new("99EAE06E-5188-43AA-BE38-B353ADC5AACF")
        {
            DataType = new VerbatimDataType("MONEY"),
        };

        public Column MyColumn304 = new("E4EEA54C-B11B-4C85-B98E-AAC299776845")
        {
            DataType = new VerbatimDataType("JSON"),
        };

        public Column MyColumn305 = new("2624C566-E9D5-4DD9-974A-BF031F73A714")
        {
            DataType = new VerbatimDataType("JSONB"),
        };

        public Column MyColumn306 = new("616314FC-E56A-424D-81EE-AE89BE650D42")
        {
            DataType = new VerbatimDataType("XML"),
        };

        public Column MyColumn307 = new("B6F006FA-2A78-4965-AC10-0D2CF05D60F0")
        {
            DataType = new VerbatimDataType("TSQUERY"),
        };

        public Column MyColumn308 = new("129FD264-AF69-4AA7-B4EF-6B9923340CD8")
        {
            DataType = new VerbatimDataType("TSVECTOR"),
        };

        public Column MyColumn309 = new("D1AE8CCD-526A-46E1-A5A0-39F25CC391CE")
        {
            DataType = new VerbatimDataType("POINT"),
        };

        public Column MyColumn310 = new("F3F38026-18CC-49EC-9D4B-3E844427A6F8")
        {
            DataType = new VerbatimDataType("LINE"),
        };

        public Column MyColumn311 = new("87C48981-BDB7-4DA7-AC4A-1E03913133FA")
        {
            DataType = new VerbatimDataType("LSEG"),
        };

        public Column MyColumn312 = new("7B535F7E-BCC3-4950-B8AF-A16EA36FB7BB")
        {
            DataType = new VerbatimDataType("BOX"),
        };

        public Column MyColumn313 = new("1C4C38CC-8266-4992-B87A-179B0AFFB526")
        {
            DataType = new VerbatimDataType("PATH"),
        };

        public Column MyColumn314 = new("448D6DE4-9D8E-47CB-99E7-9801E78A3E7F")
        {
            DataType = new VerbatimDataType("POLYGON"),
        };

        public Column MyColumn315 = new("A1E9678B-12BD-4C2F-88A5-8F203A10D4BF")
        {
            DataType = new VerbatimDataType("CIRCLE"),
        };

        public Column MyColumn316 = new("F8EF2969-0182-44C0-B9E5-58506B2353BE")
        {
            DataType = new VerbatimDataType("INET"),
        };

        public Column MyColumn317 = new("1D70B438-3A2C-4D8A-9FB8-2E39BACD2582")
        {
            DataType = new VerbatimDataType("CIDR"),
        };

        public Column MyColumn318 = new("EEB2BCA0-4D76-4B75-98C1-130A62268265")
        {
            DataType = new VerbatimDataType("MACADDR"),
        };

        public Column MyColumn319 = new("9594F5F1-D746-4EFA-A174-65E9EB82EEA0")
        {
            DataType = new VerbatimDataType("MACADDR8"),
        };

        public Column MyColumn320 = new("78AAF5EC-B0C6-4503-8FA7-A87E1D45F532")
        {
            DataType = new VerbatimDataType("decimal"),
        };

        public Column MyColumn321 = new("DC2E14B0-0FD7-44D9-92A7-39EE99358459")
        {
            DataType = new VerbatimDataType("time with time zone"),
        };

        public Column MyColumn322 = new("4C2AA5F2-893F-42F1-A39D-D81123988B2E")
        {
            DataType = new VerbatimDataType("time ( 4)"),
        };

        public Column MyColumn323 = new("710E422F-7899-45FA-8B1D-D2543519FFC1")
        {
            DataType = new VerbatimDataType("time ( 4) with time zone"),
        };

        public Column MyColumn324 = new("FB0562D8-AFBA-4939-8CD3-89878C572B56")
        {
            DataType = new VerbatimDataType("timestamp ( 4) without time zone"),
        };

        public Column MyColumn325 = new("BAABB5C1-E231-4C0B-97EC-F6BE8FBA018F")
        {
            DataType = new VerbatimDataType("timestamptz ( 4)"),
        };

        public Column MyColumn326 = new("4763C35F-BDBD-485E-9010-A950CB5A4BDB")
        {
            DataType = new VerbatimDataType("INTERVAL"),
        };

        public Column MyColumn327 = new("ECBFA2F0-98A1-4D13-9DA6-772A80DDDAC7")
        {
            DataType = new VerbatimDataType("interval  minute   to  second  (6 )"),
        };

        public Column MyColumn328 = new("36049EF7-16F2-41F3-AB8C-69D23726AD42")
        {
            DataType = new VerbatimDataType("BIT"),
        };

        public Column MyColumn329 = new("03F0A0A6-CF85-41DC-8C3A-879AE832E9AB")
        {
            DataType = new VerbatimDataType("VARBIT"),
        };

        public Column MyColumn330 = new("9EC1EF6A-9DF3-4633-889A-660F53A4866F")
        {
            DataType = new VerbatimDataType("bit  (10  )"),
        };

        public Column MyColumn331 = new("EEB5B0E8-0B1F-4EC6-B47B-068E5B303255")
        {
            DataType = new VerbatimDataType("bit varying  ( 20 )"),
        };

        public Column MyColumn332 = new("AB4454EC-A6B4-4D51-B0FA-0E667B26326D")
        {
            DataType = new VerbatimDataType("character"),
        };

        public Column MyColumn333 = new("684F2D90-9EEA-4879-B2D5-0197D32654B0")
        {
            DataType = new VerbatimDataType("character varying"),
        };

        public Column MyColumn334 = new("54B3D4DD-B3A9-4B99-915D-DFF9FE24D7C2")
        {
            DataType = new VerbatimDataType("char  (30)"),
        };

        public Column MyColumn335 = new("3631D9CB-C041-44E5-A964-C2751918C234")
        {
            DataType = new VerbatimDataType("varchar  (  40 )"),
        };

        public Column MyColumn336 = new("00DFCE94-1C1D-4FC2-BDB0-8E1306A248A2")
        {
            DataType = new VerbatimDataType("jsonb  [] [ ]"),
        };

        public Column MyColumn337 = new("9430C94B-7DED-47B1-83BB-A041F0EDEE88")
        {
            DataType = new VerbatimDataType("decimal  ( 10, 4 )  [22] []"),
        };

        public Column MyColumn338 = new("86107168-6F36-43F9-8A56-78D19049BEDA")
        {
            DataType = new VerbatimDataType("time  (  4 )  with  time  zone [11]"),
        };

        public Column MyColumn339 = new("A9863561-9309-4911-94D8-C12D21B0884E")
        {
            DataType = new VerbatimDataType($"{nameof(MyCompositeType1)}  [ 88  ]"),
        };

        public Column MyColumn340 = new("0D33ED85-9909-46E7-8369-EEE86B563519")
        {
            DataType = new VerbatimDataType($"{nameof(MyCompositeType1).Quote()}  [ 99  ]"),
        };

        public Index IDX_MyTable5_1 = new("F7F367DA-088F-48DD-BAD5-2A14A0E77F66")
        {
            Expression = $"(length({nameof(MyColumn2).Quote()} || {nameof(MyColumn1).Quote()}) + 1)",
            Method = IndexMethod.BTree,
            IncludeColumns = new[] { nameof(MyColumn4), nameof(MyColumn3), nameof(MyColumn5) },
        };
    }
}
