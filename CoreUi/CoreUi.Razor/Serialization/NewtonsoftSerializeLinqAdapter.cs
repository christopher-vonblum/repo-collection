using System;
using System.Linq.Expressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serialize.Linq.Serializers;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace CoreUi.Razor.Serialization
{
    public class NewtonsoftSerializeLinqAdapter : JsonConverter
    {
        private ExpressionSerializer expSer = new ExpressionSerializer(new Serialize.Linq.Serializers.JsonSerializer());
        
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JToken token = JToken.Load(reader);
            
            return expSer.DeserializeText(token.ToString(Formatting.None));
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                return;
            }
            
            JToken.Parse(expSer.SerializeText((Expression)value)).WriteTo(writer);
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(Expression).IsAssignableFrom(objectType);
        }
    }
}