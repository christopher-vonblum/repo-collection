namespace CVB.NET.StringConversion.Converters
{
    using System;
    using System.Collections.Concurrent;
    using System.Reflection;

    public class SimpleParsePatternSerializer<TParsable> : StringConverterBase<TParsable>
    {
        protected const string ParseFunctionName = "Parse";
        protected static readonly BindingFlags ParseFunctionBindingFlags = BindingFlags.Public | BindingFlags.Static;

        protected static ConcurrentDictionary<Type, MethodInfo> ParseMethodInfos { get; } = new ConcurrentDictionary<Type, MethodInfo>();

        public override TParsable Deserialize(string serializedValue)
        {
            return (TParsable) GetParseMethodInfo().Invoke(null, new object[] {serializedValue});
        }

        public override string Serialize(TParsable value)
        {
            return value.ToString();
        }

        protected virtual MethodInfo GetParseMethodInfo()
        {
            return ParseMethodInfos.GetOrAdd(TargetType,
                (type) =>
                    type.GetMethod(ParseFunctionName, ParseFunctionBindingFlags, null, CallingConventions.Standard, new[] {typeof (string)},
                        null));
        }
    }
}