using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Analysis.Core;

public abstract class DbModelConverter : IDbModelConverter
{
    public abstract Database FromAgnostic(Database database);

    private readonly DatabaseKind _databaseKind;

    protected DbModelConverter(DatabaseKind databaseKind)
    {
        _databaseKind = databaseKind;
    }

    protected Script ConvertScript(Script script)
    {
        script.CodePiece = ConvertCodePiece(script.CodePiece);
        return script;
    }

    protected CodePiece ConvertCodePiece(CodePiece codePiece)
    {
        return new CodePiece { Code = ((AgnosticCodePiece)codePiece).DbKindToCodeMap[_databaseKind] };
    }
}
