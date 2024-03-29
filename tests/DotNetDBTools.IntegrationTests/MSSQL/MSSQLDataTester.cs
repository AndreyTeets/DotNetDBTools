﻿using DotNetDBTools.IntegrationTests.Base;

namespace DotNetDBTools.IntegrationTests.MSSQL;

internal class MSSQLDataTester : BaseDataTester
{
    protected override string Quote(string identifier)
    {
        return $@"[{identifier}]";
    }

    protected override string BoolLiteral(bool value)
    {
        return value ? "1" : "0";
    }

    protected override string BinaryLiteral(string hexBase)
    {
        return $@"0x{hexBase}";
    }

    protected override string GuidLiteral(string guidString)
    {
        return $@"'{guidString}'";
    }

    protected override string GetSpecificDbmsTable5ExtraColumns()
    {
        return
$@",
    {Quote("MyColumn101")}"
        ;
    }

    protected override string GetSpecificDbmsTable5ExtraValues()
    {
        return
$@",
    'abc101'"
        ;
    }

    protected override string GetSpecificDbmsTable5ExtraConditions()
    {
        return
$@"
    AND {Quote("MyColumn101")} = 'abc101'"
        ;
    }
}
