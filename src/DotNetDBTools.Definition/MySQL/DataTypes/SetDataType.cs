using System.Collections.Generic;
using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.Definition.MySQL.DataTypes;

/// <summary>
/// Column is declared as 'SET(AllowedValues)'.
/// </summary>
public class SetDataType : IDataType
{
    public IEnumerable<string> AllowedValues { get; set; }
}
