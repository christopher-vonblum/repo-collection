namespace CVB.NET.Configuration.Serialization.Serializers
{
    using System.Globalization;
    using System.Reflection;

    public class FloatingPointSerializer<TParsableFloatingPoint> : SimpleParsePatternSerializer<TParsableFloatingPoint>
    {
        public override TParsableFloatingPoint Deserialize(string serializedValue)
        {
            return (TParsableFloatingPoint) GetParseMethodInfo().Invoke(null, new object[] {serializedValue, NumberStyles.AllowDecimalPoint});
        }

        protected override MethodInfo GetParseMethodInfo()
        {
            return ParseMethodInfos.GetOrAdd(ValueType,
                (type) =>
                    type.GetMethod(ParseFunctionName, ParseFunctionBindingFlags, null, CallingConventions.Standard, new[] {typeof (string), typeof (NumberStyles)},
                        null));
        }
    }
}