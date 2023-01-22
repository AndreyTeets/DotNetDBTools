using System;
using System.Collections.Generic;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.PostgreSQL;
using static DotNetDBTools.Generation.Core.Definition.CSharpDefinitionGenerationHelper;

namespace DotNetDBTools.Generation.Core.Definition;

internal class PostgreSQLTablesCSharpDefinitionGenerator : TablesCSharpDefinitionGenerator
{
    protected override void AddAdditionalColumnPropsDeclarations(List<string> propsDeclarations, Column column)
    {
        PostgreSQLColumn c = (PostgreSQLColumn)column;
        if (c.IdentityGenerationKind is not null)
            propsDeclarations.Add($@"            IdentityGenerationKind = IdentityGenerationKind.{MapIdentityGenerationKind(c.IdentityGenerationKind)},");

        if (c.IdentitySequenceOptions is not null && SequenceOptions(c.IdentitySequenceOptions) != "")
        {
            propsDeclarations.Add(
$@"            IdentitySequenceOptions = new()
            {{
{SequenceOptions(c.IdentitySequenceOptions)}
            }},");
        }
    }

    private static string MapIdentityGenerationKind(string identityGenerationKind)
    {
        return identityGenerationKind switch
        {
            "ALWAYS" => "Always",
            "BY DEFAULT" => "ByDefault",
            _ => throw new InvalidOperationException($"Invalid identityGenerationKind: '{identityGenerationKind}'")
        };
    }

    private static string SequenceOptions(PostgreSQLSequenceOptions so)
    {
        List<string> res = new();
        if (so.StartWith is not null)
            res.Add($"                StartWith = {so.StartWith},");
        if (so.IncrementBy is not null)
            res.Add($"                IncrementBy = {so.IncrementBy},");
        if (so.MinValue is not null)
            res.Add($"                MinValue = {so.MinValue},");
        if (so.MaxValue is not null)
            res.Add($"                MaxValue = {so.MaxValue},");
        if (so.Cache is not null)
            res.Add($"                Cache = {so.Cache},");
        if (so.Cycle is not null)
            res.Add($"                Cycle = {DeclareBool(so.Cycle.Value)},");

        if (res.Count > 0)
            return string.Join("\n", res);
        else
            return "";
    }
}
