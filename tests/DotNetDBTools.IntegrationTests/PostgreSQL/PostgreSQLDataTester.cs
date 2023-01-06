using DotNetDBTools.IntegrationTests.Base;

namespace DotNetDBTools.IntegrationTests.PostgreSQL;

internal class PostgreSQLDataTester : BaseDataTester
{
    protected override string Quote(string identifier)
    {
        return $@"""{identifier}""";
    }

    protected override string BoolLiteral(bool value)
    {
        return value ? "TRUE" : "FALSE";
    }

    protected override string BinaryLiteral(string hexBase)
    {
        return $@"'\x{hexBase}'";
    }

    protected override string GuidLiteral(string guidString)
    {
        return $@"'{guidString}'";
    }

    protected override string GetSpecificDbmsTable5ExtraColumns()
    {
        return
$@",
    {Quote("MyColumn101")},
    {Quote("MyColumn102")},
    {Quote("MyColumn103")},
    {Quote("MyColumn104")}"
        ;
    }

    protected override string GetSpecificDbmsTable5ExtraValues()
    {
        return
$@",
    '(abc,101)',
    'abc102',
    'Label2',
    '[2006-06-16 16:16:16,2016-06-16 16:16:16]'"
        ;
    }

    protected override string GetSpecificDbmsTable5ExtraConditions()
    {
        return
$@"
    AND {Quote("MyColumn101")} = '(abc,101)'::""MyCompositeType1""
    AND {Quote("MyColumn102")} = 'abc102'
    AND {Quote("MyColumn103")} = 'Label2'
    AND {Quote("MyColumn104")} = '[2006-06-16 16:16:16,2016-06-16 16:16:16]'"
        ;
    }
}
