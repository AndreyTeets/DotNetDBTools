using DotNetDBTools.Definition.Core;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.DefinitionParsing.Core;

internal interface IDbObjectCodeMapper
{
    public CodePiece MapToCodePiece(IDbObject dbObject);
}
