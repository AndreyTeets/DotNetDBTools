using System;
using System.Collections.Generic;
using DotNetDBTools.Analysis.Extensions;
using DotNetDBTools.Definition.Agnostic;
using DotNetDBTools.Definition.Core;
using DotNetDBTools.DefinitionParsing.Core;
using DotNetDBTools.Models.Agnostic;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.DefinitionParsing.Agnostic;

internal class AgnosticDbObjectCodeMapper : IDbObjectCodeMapper
{
    public CodePiece MapToCodePiece(IDbObject dbObject)
    {
        return dbObject switch
        {
            Definition.Agnostic.CheckConstraint ck => CreateAgnosticCodePiece(ck.Expression),
            Definition.Agnostic.Trigger trigger => CreateAgnosticCodePiece(trigger.CreateStatement),
            IView view => CreateAgnosticCodePiece(view.CreateStatement),
            IScript script => CreateAgnosticCodePiece(script.Text),
            _ => throw new InvalidOperationException($"Invalid dbObject for code mapping: {dbObject}"),
        };
    }

    public static AgnosticCodePiece CreateAgnosticCodePiece(Func<DbmsKind, string> getCodeFunc)
    {
        return new AgnosticCodePiece
        {
            Code = null,
            DbKindToCodeMap = CreateDbKindToCodeMap(getCodeFunc),
        };
    }

    private static Dictionary<DatabaseKind, string> CreateDbKindToCodeMap(Func<DbmsKind, string> getCodeFunc)
    {
        return new Dictionary<DatabaseKind, string>()
        {
            { DatabaseKind.MSSQL, getCodeFunc(DbmsKind.MSSQL)?.NormalizeLineEndings() },
            { DatabaseKind.MySQL, getCodeFunc(DbmsKind.MySQL)?.NormalizeLineEndings() },
            { DatabaseKind.PostgreSQL, getCodeFunc(DbmsKind.PostgreSQL)?.NormalizeLineEndings() },
            { DatabaseKind.SQLite, getCodeFunc(DbmsKind.SQLite)?.NormalizeLineEndings() },
        };
    }
}
