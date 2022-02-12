﻿using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Models.Agnostic;

public class AgnosticDataType : DataType
{
    public int Size { get; set; }
    public int Length { get; set; }
    public int Precision { get; set; }
    public int Scale { get; set; }
    public bool IsFixedLength { get; set; }
    public bool IsDoublePrecision { get; set; }
    public bool IsWithTimeZone { get; set; }
}
