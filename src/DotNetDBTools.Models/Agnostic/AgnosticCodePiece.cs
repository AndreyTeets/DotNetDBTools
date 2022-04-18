using System.Collections.Generic;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Models.Agnostic;

public class AgnosticCodePiece : CodePiece
{
    public Dictionary<DatabaseKind, string> DbKindToCodeMap { get; set; }
}
