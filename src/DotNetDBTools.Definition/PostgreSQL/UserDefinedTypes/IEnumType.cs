using System.Collections.Generic;
using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.Definition.PostgreSQL.UserDefinedTypes;

public interface IEnumType : IDbObject, IDataType
{
    public IEnumerable<string> AllowedValues { get; }
}
