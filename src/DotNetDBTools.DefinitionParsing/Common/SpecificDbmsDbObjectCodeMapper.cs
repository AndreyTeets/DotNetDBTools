﻿using System;
using DotNetDBTools.Definition.Common;
using DotNetDBTools.Definition.Core;
using DotNetDBTools.DefinitionParsing.Core;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.DefinitionParsing.Common
{
    internal class SpecificDbmsDbObjectCodeMapper : IDbObjectCodeMapper
    {
        public CodePiece MapToCodePiece(IDbObject dbObject)
        {
            return dbObject switch
            {
                SpecificDbmsCheckConstraint ck => CreateCodePiece(ck.Code),
                SpecificDbmsTrigger trigger => CreateCodePiece(trigger.Code),
                ISpecificDbmsView view => CreateCodePiece(view.Code),
                _ => throw new InvalidOperationException($"Invalid dbObject for code mapping: {dbObject}")
            };
        }

        private static CodePiece CreateCodePiece(string code)
        {
            return new CodePiece { Code = code.NormalizeLineEndings() };
        }
    }
}
