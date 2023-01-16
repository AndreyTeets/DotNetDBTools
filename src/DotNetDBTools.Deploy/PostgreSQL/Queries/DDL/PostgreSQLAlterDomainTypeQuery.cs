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
        string ad = $@"ALTER DOMAIN ""{typeDiff.NewTypeName}""";
        List<string> definitions = new();
        if (typeDiff.NewTypeName != typeDiff.OldTypeName)
            definitions.Add($@"ALTER DOMAIN ""{typeDiff.OldTypeName}"" RENAME TO ""{typeDiff.NewTypeName}"";");
        if (typeDiff.NotNullToSet != null && typeDiff.NotNullToSet == false)
            definitions.Add($@"{ad} DROP NOT NULL;");
        if (typeDiff.NotNullToSet != null && typeDiff.NotNullToSet == true)
            definitions.Add($@"{ad} SET NOT NULL;");
        if (typeDiff.DefaultToDrop != null)
            definitions.Add($@"{ad} DROP DEFAULT;");
        if (typeDiff.DefaultToSet != null)
            definitions.Add($@"{ad} SET DEFAULT {typeDiff.DefaultToSet.Code};");
        foreach (CheckConstraint ck in typeDiff.CheckConstraintsToDrop)
            definitions.Add($@"{ad} DROP CONSTRAINT ""{ck.Name}"";");
        foreach (CheckConstraint ck in typeDiff.CheckConstraintsToCreate)
            definitions.Add($@"{ad} ADD CONSTRAINT ""{ck.Name}"" CHECK ({ck.GetExpression()});");
        return string.Join("\n", definitions);
    }
}
