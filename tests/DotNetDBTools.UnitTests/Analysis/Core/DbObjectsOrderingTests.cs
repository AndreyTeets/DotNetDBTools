using System;
using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Analysis.Extensions;
using DotNetDBTools.Models.Core;
using FluentAssertions;
using Xunit;

namespace DotNetDBTools.UnitTests.Analysis.Core;

public class DbObjectsOrderingTests
{
    [Fact]
    public void OrderByDependencies_ProduceCorrectResultOnValidNonEmptyInput()
    {
        DbObject x01 = CreateTestDbObject("01", new DbObject[] { });
        DbObject x02 = CreateTestDbObject("02", new DbObject[] { });
        DbObject x03 = CreateTestDbObject("03", new DbObject[] { });
        DbObject x04 = CreateTestDbObject("04", new DbObject[] { });
        DbObject x05 = CreateTestDbObject("05", new DbObject[] { });
        DbObject x07 = CreateTestDbObject("07", new DbObject[] { x02, x03 });
        DbObject x08 = CreateTestDbObject("08", new DbObject[] { x05 });
        DbObject x09 = CreateTestDbObject("09", new DbObject[] { x07 });
        DbObject x10 = CreateTestDbObject("10", new DbObject[] { x09 });
        DbObject x11 = CreateTestDbObject("11", new DbObject[] { x09 });
        DbObject x06 = CreateTestDbObject("06", new DbObject[] { x01, x02, x07, x09, x10 });
        List<DbObject> dbObjects = new() { x01, x02, x03, x04, x05, x06, x07, x08, x09, x10, x11 };

        string[] expectedDependenciesFirstOrder = new string[]
        {
            "02", "03",
            "07",
            "09",
            "01", "05", "10",
            "04", "06", "08", "11",
        };
        string[] expectedDependenciesLastOrder = new string[]
        {
            "04", "06", "08", "11",
            "01", "05", "10",
            "09",
            "07",
            "02", "03",
        };

        IEnumerable<string> actualDependenciesFirstOrder = dbObjects.OrderByDependenciesFirst().Select(x => x.Name);
        IEnumerable<string> actualDependenciesLastOrder = dbObjects.OrderByDependenciesLast().Select(x => x.Name);

        actualDependenciesFirstOrder.Should().BeEquivalentTo(expectedDependenciesFirstOrder, options => options.WithStrictOrdering());
        actualDependenciesLastOrder.Should().BeEquivalentTo(expectedDependenciesLastOrder, options => options.WithStrictOrdering());
    }

    [Fact]
    public void OrderByDependencies_ProduceCorrectResultOnEmptyInput()
    {
        IEnumerable<DbObject> dbObjects = new List<DbObject>();

        string[] expectedDependenciesFirstOrder = new string[] { };
        string[] expectedDependenciesLastOrder = new string[] { };

        IEnumerable<string> actualDependenciesFirstOrder = dbObjects.OrderByDependenciesFirst().Select(x => x.Name);
        IEnumerable<string> actualDependenciesLastOrder = dbObjects.OrderByDependenciesLast().Select(x => x.Name);

        actualDependenciesFirstOrder.Should().BeEquivalentTo(expectedDependenciesFirstOrder, options => options.WithStrictOrdering());
        actualDependenciesLastOrder.Should().BeEquivalentTo(expectedDependenciesLastOrder, options => options.WithStrictOrdering());
    }

    [Fact]
    public void OrderByDependencies_ThrowOnInvalidInput()
    {
        DbObject x01 = CreateTestDbObject("01", new DbObject[] { });
        DbObject x02 = CreateTestDbObject("02", new DbObject[] { x01 });
        DbObject x03 = CreateTestDbObject("03", new DbObject[] { x02 });
        x01.DependsOn.Add(x03);
        List<DbObject> dbObjects = new() { x01, x02, x03 };

        string expectedExceptionMessageWildcard = "Invalid objects dependencies graph*";
        dbObjects.Invoking(x => x.OrderByDependenciesFirst()).Should().Throw<Exception>().WithMessage(expectedExceptionMessageWildcard);
        dbObjects.Invoking(x => x.OrderByDependenciesLast()).Should().Throw<Exception>().WithMessage(expectedExceptionMessageWildcard);
    }

    private static DbObject CreateTestDbObject(string num, DbObject[] dependsOn)
    {
        return new TestDbObject()
        {
            ID = new Guid($"{num}000000-0000-0000-0000-000000000000"),
            Name = num,
            DependsOn = dependsOn.ToList(),
        };
    }

    private class TestDbObject : DbObject { }
}
