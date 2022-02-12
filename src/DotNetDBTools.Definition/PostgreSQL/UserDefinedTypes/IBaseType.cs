using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.Definition.PostgreSQL.UserDefinedTypes;

/// <summary>
/// Creation/Deletion/Modification of base types is not handled by DotNetDBTools, it's only required for referencing in other objects.
/// </summary>
public interface IBaseType : IDataType
{
}
