using System.Collections.Generic;

namespace DotNetDBTools.Models.Core
{
    public class AgnosticCodePiece : CodePiece
    {
        public IDictionary<DatabaseKind, string> DbKindToCodeMap { get; set; }
    }
}
