using System;
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
                Definition.Common.SpecificDbmsCheckConstraint ck => new CodePiece { Code = ck.Code },
                Definition.Common.SpecificDbmsTrigger trigger => new CodePiece { Code = trigger.Code },
                Definition.Common.ISpecificDbmsView view => new CodePiece { Code = view.Code },
                _ => throw new InvalidOperationException($"Invalid dbObject for code mapping: {dbObject}")
            };
        }
    }
}
