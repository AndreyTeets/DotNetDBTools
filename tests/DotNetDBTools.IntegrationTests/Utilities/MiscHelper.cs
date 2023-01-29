using DotNetDBTools.Models.Core;
using FluentAssertions.Equivalency;

namespace DotNetDBTools.IntegrationTests.Utilities;

internal static class MiscHelper
{
    public static EquivalencyAssertionOptions<Database> ExcludingDependencies(this EquivalencyAssertionOptions<Database> options)
    {
        return options
            .Excluding(mi => mi.Name == nameof(DbObject.Parent) && mi.DeclaringType == typeof(DbObject))
            .Excluding(mi => mi.Name == nameof(CodePiece.DependsOn) && mi.DeclaringType == typeof(CodePiece))
            .Excluding(mi => mi.Name == nameof(DataType.DependsOn) && mi.DeclaringType == typeof(DataType))
            .Excluding(mi => mi.Name == nameof(PrimaryKey.DependsOn) && mi.DeclaringType == typeof(PrimaryKey))
            .Excluding(mi => mi.Name == nameof(UniqueConstraint.DependsOn) && mi.DeclaringType == typeof(UniqueConstraint))
            .Excluding(mi => mi.Name == nameof(ForeignKey.DependsOn) && mi.DeclaringType == typeof(ForeignKey))
            .Excluding(mi => mi.Name == nameof(Index.DependsOn) && mi.DeclaringType == typeof(Index));
    }
}
