﻿namespace DotNetDBTools.CodeParsing.Models;

public class ColumnInfo : ObjectInfo
{
    public string DataType { get; set; }
    public bool NotNull { get; set; }
    public string Default { get; set; }
    public bool Identity { get; set; }
    public bool PrimaryKey { get; set; }
    public bool Unique { get; set; }
    public string IdentityGenerationKind { get; set; }
    public SequenceOptions IdentitySequenceOptions { get; set; }
}
