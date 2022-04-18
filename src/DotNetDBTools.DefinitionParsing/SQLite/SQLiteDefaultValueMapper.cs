using DotNetDBTools.Analysis.SQLite;
using DotNetDBTools.Definition.Core;
using DotNetDBTools.Definition.SQLite;
using DotNetDBTools.DefinitionParsing.Core;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.DefinitionParsing.SQLite;

internal class SQLiteDefaultValueMapper : DefaultValueMapper
{
    public override CodePiece MapToDefaultValueModel(IDefaultValue defaultValue)
    {
        if (defaultValue is null)
            return new CodePiece { Code = null };
        if (defaultValue is VerbatimDefaultValue vdv)
            return new CodePiece { Code = vdv.Value };
        return new SQLiteDefaultValueConverter().Convert(CreateCSharpDefaultValueModel(defaultValue));
    }
}
