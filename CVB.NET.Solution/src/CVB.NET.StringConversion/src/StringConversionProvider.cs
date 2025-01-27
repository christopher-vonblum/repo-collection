namespace CVB.NET.StringConversion
{
    using System;
    using System.Collections.Concurrent;
    using Base;

    public class StringConversionProvider : IStringConversionProvider, IConfigurableStringConversionProvider
    {
        private ConcurrentDictionary<Type, IStringConverter> Converters = new ConcurrentDictionary<Type, IStringConverter>();

        public void AddStringConverter(Type converterType)
        {
            IStringConverter converter = (IStringConverter) converterType.GetConstructor(Type.EmptyTypes).Invoke(null);
            Converters.GetOrAdd(converter.TargetType, converter);
        }

        public bool HasConverter(Type targetType)
        {
            return Converters.ContainsKey(targetType);
        }

        public object Deserialize(string value, Type targetType)
        {
            return Converters[targetType].Deserialize(value);
        }

        public string Serialize(object obj)
        {
            return Converters[obj.GetType()].Serialize(obj);
        }
    }
}