namespace CVB.NET.Configuration.Serialization.Base
{
    using System;
    using System.Globalization;
    using System.Runtime.Serialization;
    using System.Threading;
    using NET.Aspects.ExceptionHandling;

    public abstract class ConfigStringSerializerBase<TValueType> : IConfigStringSerializer<TValueType>
    {
        protected static CultureInfo SerializationCulture = new CultureInfo("en-US");

        public Type ValueType => typeof (TValueType);

        [WrapAndThrowExceptionAspect(typeof (SerializationException), "Error while deserializing configuration property.")]
        public object DynamicDeserialize(string serializedValue)
        {
            CultureInfo restoreCulture = Thread.CurrentThread.CurrentCulture;

            Thread.CurrentThread.CurrentCulture = SerializationCulture;

            object deserialized = Deserialize(serializedValue);

            Thread.CurrentThread.CurrentCulture = restoreCulture;

            return deserialized;
        }

        public abstract TValueType Deserialize(string serializedValue);

        [WrapAndThrowExceptionAspect(typeof (SerializationException), "Error while serializing configuration property.")]
        public string DynamicSerialize(object value)
        {
            CultureInfo restoreCulture = Thread.CurrentThread.CurrentCulture;

            Thread.CurrentThread.CurrentCulture = SerializationCulture;

            string serialized = Serialize((TValueType) value);

            Thread.CurrentThread.CurrentCulture = restoreCulture;

            return serialized;
        }

        public abstract string Serialize(TValueType value);
    }
}