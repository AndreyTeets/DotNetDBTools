using System;
using System.Collections.Generic;
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
            Definition.Agnostic.CheckConstraint ck => new AgnosticCodePiece
            {
                Code = null,
                DbKindToCodeMap = CreateDbKindToCodeMap(ck.Code),
            },
            Definition.Agnostic.Trigger trigger => new AgnosticCodePiece
            {
                Code = null,
                DbKindToCodeMap = CreateDbKindToCodeMap(trigger.Code),
            },
            IView view => new AgnosticCodePiece
            {
                Code = null,
                DbKindToCodeMap = CreateDbKindToCodeMap(view.Code),
            },
            IScript script => new AgnosticCodePiece
            {
                Code = null,
                DbKindToCodeMap = CreateDbKindToCodeMap(script.Code),
            },
            _ => throw new InvalidOperationException($"Invalid dbObject for code mapping: {dbObject}"),
        };
    }

    private static Dictionary<DatabaseKind, string> CreateDbKindToCodeMap(Func<DbmsKind, string> getCodeFunc)
    {
        return new Dictionary<DatabaseKind, string>()
        {
            { DatabaseKind.MSSQL, getCodeFunc(DbmsKind.MSSQL).NormalizeLineEndings() },
            { DatabaseKind.MySQL, getCodeFunc(DbmsKind.MySQL).NormalizeLineEndings() },
            { DatabaseKind.PostgreSQL, getCodeFunc(DbmsKind.PostgreSQL).NormalizeLineEndings() },
            { DatabaseKind.SQLite, getCodeFunc(DbmsKind.SQLite).NormalizeLineEndings() },
        };
    }
}
