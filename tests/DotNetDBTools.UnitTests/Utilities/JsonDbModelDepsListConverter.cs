using System;
using System.Collections.Generic;
using DotNetDBTools.Models.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DotNetDBTools.UnitTests.Utilities;

public class JsonDbModelDepsListConverter : JsonConverter
{
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        if (value is List<DbObject> dbObjects)
        {
            JArray resultArray = new();
            foreach (DbObject dbObject in dbObjects)
            {
                JValue dbObjectDisplayedValue = new(MiscHelper.GetDbObjectDisplayText(dbObject));
                resultArray.Add(dbObjectDisplayedValue);
            }
            resultArray.WriteTo(writer);
        }
        else
        {
            throw new NotImplementedException("Not supposed to be called.");
        }
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        throw new NotImplementedException("Not supposed to be called.");
    }

    public override bool CanRead
    {
        get { throw new NotImplementedException("Not supposed to be called."); }
    }

    public override bool CanConvert(Type objectType)
    {
        throw new NotImplementedException("Not supposed to be called.");
    }
}
