using System.Collections.Generic;
using DotNetDBTools.Generation.Core.Sql;
using DotNetDBTools.Models.PostgreSQL;

namespace DotNetDBTools.Generation.PostgreSQL.Sql;

internal class PostgreSQLSequenceStatementsGenerator : StatementsGenerator<PostgreSQLSequence>
{
    protected override string GetCreateSqlImpl(PostgreSQLSequence sequence)
    {
        string res =
$@"{GetIdDeclarationText(sequence, 0)}CREATE SEQUENCE ""{sequence.Name}""
{GetSequenceDefinitionsText(sequence)};";

        return res;
    }

    protected override string GetDropSqlImpl(PostgreSQLSequence sequence)
    {
        return $@"DROP SEQUENCE ""{sequence.Name}"";";
    }

    private string GetSequenceDefinitionsText(PostgreSQLSequence sequence)
    {
        List<string> definitions = new();
        definitions.Add($@"    AS {sequence.DataType.Name}");
        definitions.Add($@"    {SequenceOptions(sequence.Options)}");
        if (sequence.OwnedBy != (null, null))
            definitions.Add($@"    OWNED BY ""{sequence.OwnedBy.TableName}"".""{sequence.OwnedBy.ColumnName}""");
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
