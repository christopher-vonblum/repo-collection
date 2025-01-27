namespace CVB.NET.Configuration.Serialization.Serializers
{
    using System;
    using System.Collections.Concurrent;
    using System.Reflection;
    using Base;

    public class SimpleParsePatternSerializer<TParsable> : ConfigStringSerializerBase<TParsable>
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
            return ParseMethodInfos.GetOrAdd(ValueType,
                (type) =>
                    type.GetMethod(ParseFunctionName, ParseFunctionBindingFlags, null, CallingConventions.Standard, new[] {typeof (string)},
                        null));
        }
    }
}