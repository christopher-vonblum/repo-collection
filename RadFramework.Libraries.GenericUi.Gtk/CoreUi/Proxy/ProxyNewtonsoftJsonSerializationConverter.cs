using System;
using CoreUi.Proxy.Factory;
using Newtonsoft.Json;

namespace CoreUi.Proxy
{
    public class ProxyNewtonsoftJsonSerializationConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotSupportedException("CustomCreationConverter should only be used while deserializing.");
        }
        public override object ReadJson(
            JsonReader reader,
            Type objectType,
            object existingValue,
            JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return (object) null;
            object o = ProxyFactory.CreateProxyOrValue(objectType);
            serializer.Populate(reader, (object) o);
            return (object) o;
        }
        public override bool CanConvert(Type objectType)
        {
            return objectType.IsInterface;
        }
        public override bool CanWrite
        {
            get
            {
                return false;
            }
        }
    }
}