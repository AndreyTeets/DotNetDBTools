using System.Collections.Generic;

namespace DotNetDBTools.Models.Core;

public class CodePiece
{
    public string Code { get; set; }

    public List<DbObject> DependsOn { get; set; } = new();
}
