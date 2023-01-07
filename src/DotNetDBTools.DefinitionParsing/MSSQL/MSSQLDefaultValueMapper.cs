using DotNetDBTools.Analysis;
using DotNetDBTools.Definition.Core;
using DotNetDBTools.Definition.MSSQL;
using DotNetDBTools.DefinitionParsing.Core;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.DefinitionParsing.MSSQL;

internal class MSSQLDefaultValueMapper : DefaultValueMapper
{
    public override CodePiece MapToDefaultValueModel(IDefaultValue defaultValue)
    {
        if (defaultValue is null)
            return new CodePiece { Code = null };
        if (defaultValue is VerbatimDefaultValue vdv)
            return new CodePiece { Code = vdv.Expression };
        return new AnalysisManager().ConvertDefaultValue(CreateCSharpDefaultValueModel(defaultValue), DatabaseKind.MSSQL);
    }
}
