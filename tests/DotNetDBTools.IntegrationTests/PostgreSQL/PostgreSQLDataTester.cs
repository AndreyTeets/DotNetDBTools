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
    {Quote("MyColumn13")},
    {Quote("MyColumn14")},
    {Quote("MyColumn15")},
    {Quote("MyColumn16")}"
        ;
    }

    protected override string GetSpecificDbmsTable5ExtraValues()
    {
        return
$@",
    '(abc,13)',
    'abc14',
    'Label2',
    '[2006-06-16 16:16:16,2016-06-16 16:16:16]'"
        ;
    }

    protected override string GetSpecificDbmsTable5ExtraConditions()
    {
        return
$@"
    AND {Quote("MyColumn13")} = '(abc,13)'::""MyCompositeType1""
    AND {Quote("MyColumn14")} = 'abc14'
    AND {Quote("MyColumn15")} = 'Label2'
    AND {Quote("MyColumn16")} = '[2006-06-16 16:16:16,2016-06-16 16:16:16]'"
        ;
    }
}
