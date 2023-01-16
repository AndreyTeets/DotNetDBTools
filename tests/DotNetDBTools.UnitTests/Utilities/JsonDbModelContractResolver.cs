using System.Collections.Generic;
using System.Reflection;
using DotNetDBTools.Models.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DotNetDBTools.UnitTests.Utilities;

public class JsonDbModelContractResolver : DefaultContractResolver
{
    public static new readonly JsonDbModelContractResolver Instance = new();

    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    {
        JsonProperty property = base.CreateProperty(member, memberSerialization);

        if (property.PropertyType == typeof(List<DbObject>)
            && (property.DeclaringType == typeof(CodePiece) && property.PropertyName == nameof(CodePiece.DependsOn)
                || property.DeclaringType == typeof(DataType) && property.PropertyName == nameof(DataType.DependsOn)))
        {
            property.Converter = new JsonDbModelDepsListConverter();
        }

        return property;
    }
}
