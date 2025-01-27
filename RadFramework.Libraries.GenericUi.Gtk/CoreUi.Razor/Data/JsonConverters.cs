using CoreUi.Proxy;
using CoreUi.Razor.Serialization;
using Newtonsoft.Json;

namespace CoreUi.Razor.Data
{
    public static class JsonConverters
    {
        public static JsonConverter[] Converters = new JsonConverter[]{new NewtonsoftSerializeLinqAdapter(), new ProxyNewtonsoftJsonSerializationConverter()};
    }
}