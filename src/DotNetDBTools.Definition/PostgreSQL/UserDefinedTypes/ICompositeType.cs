using System.Collections.Generic;
using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.Definition.PostgreSQL.UserDefinedTypes;

public interface ICompositeType : IDbObject, IDataType
{
    public IDictionary<string, IDataType> Attributes { get; }
}
