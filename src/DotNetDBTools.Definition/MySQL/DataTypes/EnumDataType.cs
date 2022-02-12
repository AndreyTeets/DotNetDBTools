using System.Collections.Generic;
using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.Definition.MySQL.DataTypes;

/// <summary>
/// Column is declared as 'ENUM(AllowedValues)'.
/// </summary>
public class EnumDataType : IDataType
{
    public IEnumerable<string> AllowedValues { get; set; }
}
