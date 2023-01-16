using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using DotNetDBTools.Models.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DotNetDBTools.UnitTests.Utilities;

public class JsonDbModelReferenceResolver : IReferenceResolver
{
    private readonly Dictionary<object, string> _nonDbObjectToReferenceMap = new();
    private readonly Dictionary<DbObject, string> _dbObjectToReferenceMap = new();

    public void AddReference(object context, string reference, object value)
    {
        throw new System.NotImplementedException();
    }

    public string GetReference(object context, object value)
    {
        if (value is DbObject dbObject)
        {
            if (!_dbObjectToReferenceMap.ContainsKey(dbObject))
                _dbObjectToReferenceMap.Add(dbObject, MiscHelper.GetDbObjectDisplayText(dbObject));
            return _dbObjectToReferenceMap[dbObject];
        }
        else
        {
            if (!_nonDbObjectToReferenceMap.ContainsKey(value))
                _nonDbObjectToReferenceMap.Add(value, ComputeObjectHash(value));
            return _nonDbObjectToReferenceMap[value];
        }
    }

    public bool IsReferenced(object context, object value)
    {
        if (value is DbObject dbObject)
            return _dbObjectToReferenceMap.ContainsKey(dbObject);
        else
            return _nonDbObjectToReferenceMap.ContainsKey(value);
    }

    public object ResolveReference(object context, string reference)
    {
        throw new System.NotImplementedException();
    }

    private static string ComputeObjectHash(object value)
    {
        JsonSerializerSettings settings = new()
        {
            ContractResolver = new IgnoreDepsContractResolver(),
        };
        string valueStr = JsonConvert.SerializeObject(value, settings).NormalizeLineEndings();

        using SHA256 sha256 = SHA256.Create();
        byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(valueStr));

        string hash = "";
        foreach (byte b in hashBytes)
            hash += $"{b:X2}";
        return hash.Substring(0, 16);
    }

    public class IgnoreDepsContractResolver : DefaultContractResolver
    {
        public static new readonly JsonDbModelContractResolver Instance = new();

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);

            if (property.PropertyType == typeof(List<DbObject>)
                && (property.DeclaringType == typeof(CodePiece) && property.PropertyName == nameof(CodePiece.DependsOn)
                    || property.DeclaringType == typeof(DataType) && property.PropertyName == nameof(DataType.DependsOn))
                || property.PropertyType == typeof(DbObject)
                && property.DeclaringType == typeof(DbObject) && property.PropertyName == nameof(DbObject.Parent))
            {
                property.ShouldSerialize = x => false;
            }

            return property;
        }
    }
}
