using DotNetDBTools.Analysis;
using DotNetDBTools.Definition.Core;
using DotNetDBTools.Definition.PostgreSQL;
using DotNetDBTools.DefinitionParsing.Core;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.DefinitionParsing.PostgreSQL;

internal class PostgreSQLDefaultValueMapper : DefaultValueMapper
{
    public override CodePiece MapToDefaultValueModel(IDefaultValue defaultValue)
    {
        if (defaultValue is null)
            return null;
        if (defaultValue is VerbatimDefaultValue vdv)
            return new CodePiece { Code = vdv.Expression };
        return new AnalysisManager().ConvertDefaultValue(CreateCSharpDefaultValueModel(defaultValue), DatabaseKind.PostgreSQL);
    }
}
