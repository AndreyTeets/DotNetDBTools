using DotNetDBTools.IntegrationTests.Base;

namespace DotNetDBTools.IntegrationTests.MySQL;

public class MySQLDataTester : BaseDataTester
{
    protected override string Quote(string identifier)
    {
        return @$"`{identifier}`";
    }

    protected override string BoolLiteral(bool value)
    {
        return value ? "TRUE" : "FALSE";
    }

    protected override string BinaryLiteral(string hexBase)
    {
        return @$"(0x{hexBase})";
    }

    protected override string GuidLiteral(string guidString)
    {
        return @$"(0x{guidString.Replace("-", "").ToLower()})";
    }

    protected override string GetSpecificDbmsTable5ExtraColumns()
    {
        return "";
    }

    protected override string GetSpecificDbmsTable5ExtraValues()
    {
        return "";
    }

    protected override string GetSpecificDbmsTable5ExtraConditions()
    {
        return "";
    }
}
