using System.Collections.Generic;

namespace DotNetDBTools.CodeParsing.Models;

public class TypeInfo : ObjectInfo
{
    public TypeType TypeType { get; set; }

    public Dictionary<string, string> Attributes { get; set; } = new();

    public List<string> AllowedValues { get; set; } = new();

    public string UnderlyingType { get; set; }
    public bool NotNull { get; set; }
    public string Default { get; set; }
    public List<ConstraintInfo> CheckConstraints { get; set; } = new();

    public string Subtype { get; set; }
    public string SubtypeOperatorClass { get; set; }
    public string Collation { get; set; }
    public string CanonicalFunction { get; set; }
    public string SubtypeDiff { get; set; }
    public string MultirangeTypeName { get; set; }
}
