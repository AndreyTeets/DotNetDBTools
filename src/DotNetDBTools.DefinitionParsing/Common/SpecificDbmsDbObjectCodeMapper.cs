using System;
using DotNetDBTools.Analysis.Extensions;
using DotNetDBTools.Definition.Common;
using DotNetDBTools.Definition.Core;
using DotNetDBTools.DefinitionParsing.Core;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.DefinitionParsing.Common;

internal class SpecificDbmsDbObjectCodeMapper : IDbObjectCodeMapper
{
    public CodePiece MapToCodePiece(IDbObject dbObject)
    {
        return dbObject switch
        {
            SpecificDbmsCheckConstraint ck => CreateCodePiece(ck.Expression),
            SpecificDbmsTrigger trigger => CreateCodePiece(trigger.CreateStatement),
            ISpecificDbmsView view => CreateCodePiece(view.CreateStatement),
            ISpecificDbmsScript script => CreateCodePiece(script.Text),
            _ => throw new InvalidOperationException($"Invalid dbObject for code mapping: {dbObject}")
        };
    }

    private static CodePiece CreateCodePiece(string code)
    {
        return new CodePiece { Code = code.NormalizeLineEndings() };
    }
}
