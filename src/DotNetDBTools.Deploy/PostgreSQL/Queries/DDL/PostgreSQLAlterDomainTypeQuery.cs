using System.Collections.Generic;
using DotNetDBTools.Deploy.Core.Queries;
using DotNetDBTools.Generation.Core;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.PostgreSQL.UserDefinedTypes;

namespace DotNetDBTools.Deploy.PostgreSQL.Queries.DDL;

internal class PostgreSQLAlterDomainTypeQuery : NoParametersQuery
{
    public override string Sql => _sql;
    private readonly string _sql;

    public PostgreSQLAlterDomainTypeQuery(PostgreSQLDomainTypeDiff typeDiff)
    {
        _sql = GetSql(typeDiff);
    }

    private static string GetSql(PostgreSQLDomainTypeDiff typeDiff)
    {
        string ad = $@"ALTER DOMAIN ""{typeDiff.NewName}""";
        List<string> definitions = new();
        if (typeDiff.NewName != typeDiff.OldName)
            definitions.Add($@"ALTER DOMAIN ""{typeDiff.OldName}"" RENAME TO ""{typeDiff.NewName}"";");
        if (typeDiff.NotNullToSet is not null && typeDiff.NotNullToSet == false)
            definitions.Add($@"{ad} DROP NOT NULL;");
        if (typeDiff.NotNullToSet is not null && typeDiff.NotNullToSet == true)
            definitions.Add($@"{ad} SET NOT NULL;");
        if (typeDiff.DefaultToDrop is not null)
            definitions.Add($@"{ad} DROP DEFAULT;");
        if (typeDiff.DefaultToSet is not null)
            definitions.Add($@"{ad} SET DEFAULT {typeDiff.DefaultToSet.Code};");
        foreach (CheckConstraint ck in typeDiff.CheckConstraintsToDrop)
            definitions.Add($@"{ad} DROP CONSTRAINT ""{ck.Name}"";");
        foreach (CheckConstraint ck in typeDiff.CheckConstraintsToCreate)
            definitions.Add($@"{ad} ADD CONSTRAINT ""{ck.Name}"" CHECK ({ck.GetExpression()});");
        return string.Join("\n", definitions);
    }
}
