using DotNetDBTools.IntegrationTests.Base;

namespace DotNetDBTools.IntegrationTests.SQLite;

internal class SQLiteDataTester : BaseDataTester
{
    protected override string Quote(string identifier)
    {
        return $@"[{identifier}]";
    }

    protected override string BoolLiteral(bool value)
    {
        return value ? "TRUE" : "FALSE";
    }

    protected override string BinaryLiteral(string hexBase)
    {
        return $@"X'{hexBase}'";
    }

    protected override string GuidLiteral(string guidString)
    {
        return $@"X'{guidString.Replace("-", "").ToLower()}'";
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
