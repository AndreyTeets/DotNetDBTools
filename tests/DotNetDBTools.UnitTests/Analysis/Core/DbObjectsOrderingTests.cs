using System;
using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Analysis.Core;
using DotNetDBTools.Models.Core;
using FluentAssertions;
using Xunit;

namespace DotNetDBTools.UnitTests.Analysis.Core
{
    public class DbObjectsOrderingTests
    {
        [Fact]
        public void OrderByDependencies_ProduceCorrectResultOnValidNonEmptyInput()
        {
            DBObject x01 = CreateTestDbObject("01", new DBObject[] { });
            DBObject x02 = CreateTestDbObject("02", new DBObject[] { });
            DBObject x03 = CreateTestDbObject("03", new DBObject[] { });
            DBObject x04 = CreateTestDbObject("04", new DBObject[] { });
            DBObject x05 = CreateTestDbObject("05", new DBObject[] { });
            DBObject x07 = CreateTestDbObject("07", new DBObject[] { x02, x03 });
            DBObject x08 = CreateTestDbObject("08", new DBObject[] { x05 });
            DBObject x09 = CreateTestDbObject("09", new DBObject[] { x07 });
            DBObject x10 = CreateTestDbObject("10", new DBObject[] { x09 });
            DBObject x11 = CreateTestDbObject("11", new DBObject[] { x09 });
            DBObject x06 = CreateTestDbObject("06", new DBObject[] { x01, x02, x07, x09, x10 });
            List<DBObject> dbObjects = new() { x01, x02, x03, x04, x05, x06, x07, x08, x09, x10, x11 };

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
            IEnumerable<DBObject> dbObjects = new List<DBObject>();

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
            DBObject x01 = CreateTestDbObject("01", new DBObject[] { });
            DBObject x02 = CreateTestDbObject("02", new DBObject[] { x01 });
            DBObject x03 = CreateTestDbObject("03", new DBObject[] { x02 });
            x01.DependsOn.Add(x03);
            List<DBObject> dbObjects = new() { x01, x02, x03 };

            string expectedExceptionMessageWildcard = "Invalid objects dependencies graph*";
            dbObjects.Invoking(x => x.OrderByDependenciesFirst()).Should().Throw<Exception>().WithMessage(expectedExceptionMessageWildcard);
            dbObjects.Invoking(x => x.OrderByDependenciesLast()).Should().Throw<Exception>().WithMessage(expectedExceptionMessageWildcard);
        }

        private static DBObject CreateTestDbObject(string num, DBObject[] dependsOn)
        {
            return new TestDbObject()
            {
                ID = new Guid($"{num}000000-0000-0000-0000-000000000000"),
                Name = num,
                DependsOn = dependsOn.ToList(),
            };
        }

        private class TestDbObject : DBObject { }
    }
}
