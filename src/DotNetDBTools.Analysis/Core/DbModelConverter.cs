using DotNetDBTools.Models.Agnostic;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Analysis.Core;

public abstract class DbModelConverter : IDbModelConverter
{
    protected readonly IDbModelPostProcessor DbModelPostProcessor;

    private readonly DatabaseKind _databaseKind;

    protected DbModelConverter(
        DatabaseKind databaseKind,
        IDbModelPostProcessor dbModelPostProcessor)
    {
        _databaseKind = databaseKind;
        DbModelPostProcessor = dbModelPostProcessor;
    }

    public Database FromAgnostic(Database database)
    {
        Database specificDbmsDatabase = ConvertDatabase((AgnosticDatabase)database);
        DbModelPostProcessor.Do_CreateDbModelFromAgnostic_PostProcessing(specificDbmsDatabase);
        return specificDbmsDatabase;
    }

    protected abstract Database ConvertDatabase(AgnosticDatabase database);

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
