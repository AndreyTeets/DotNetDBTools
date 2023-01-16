using System.Collections.Generic;
using DotNetDBTools.Deploy.Core.Queries;
using DotNetDBTools.Models.PostgreSQL;

namespace DotNetDBTools.Deploy.PostgreSQL.Queries.DDL;

internal class PostgreSQLAlterSequenceQuery : NoParametersQuery
{
    public override string Sql => _sql;
    private readonly string _sql;

    public PostgreSQLAlterSequenceQuery(PostgreSQLSequenceDiff sequenceDiff)
    {
        _sql = GetSql(sequenceDiff);
    }

    private static string GetSql(PostgreSQLSequenceDiff sDiff)
    {
        List<string> definitions = new();
        if (sDiff.NewSequenceName != sDiff.OldSequenceName)
            definitions.Add($@"ALTER SEQUENCE ""{sDiff.OldSequenceName}"" RENAME TO ""{sDiff.NewSequenceName}"";");

        List<string> subdefinitions = new();
        if (sDiff.DataTypeToSet != null)
            subdefinitions.Add($@"    AS {sDiff.DataTypeToSet.Name}");
        if (sDiff.OptionsToSet != null && SequenceOptions(sDiff.OptionsToSet) != "")
            subdefinitions.Add($@"    {SequenceOptions(sDiff.OptionsToSet)}");
        if (sDiff.OwnedByToDrop != (null, null))
            subdefinitions.Add($@"    OWNED BY NONE");
        if (sDiff.OwnedByToSet != (null, null))
            subdefinitions.Add($@"    OWNED BY ""{sDiff.OwnedByToSet.TableName}"".""{sDiff.OwnedByToSet.ColumnName}""");

        if (subdefinitions.Count > 0)
        {
            definitions.Add(
$@"ALTER SEQUENCE ""{sDiff.NewSequenceName}""
{string.Join("\n", subdefinitions)}");
        }

        return string.Join("\n", definitions);
    }

    private static string SequenceOptions(PostgreSQLSequenceOptions so)
    {
        List<string> res = new();
        if (so.StartWith != null)
            res.Add($"START {so.StartWith}");
        if (so.IncrementBy != null)
            res.Add($"INCREMENT {so.IncrementBy}");
        if (so.MinValue != null)
            res.Add($"MINVALUE {so.MinValue}");
        if (so.MaxValue != null)
            res.Add($"MAXVALUE {so.MaxValue}");
        if (so.Cache != null)
            res.Add($"CACHE {so.Cache}");
        if (so.Cycle != null)
            res.Add(so.Cycle.Value ? $"CYCLE" : "NO CYCLE");

        if (res.Count > 0)
            return string.Join(" ", res);
        else
            return "";
    }
}
