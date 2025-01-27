namespace CVB.NET.Configuration.Serialization.Base
{
    using System;
    using NET.Aspects.Validation.BuildTime;

    [RequiresConstructorContract]
    public interface IConfigStringSerializer
    {
        Type ValueType { get; }

        object DynamicDeserialize(string serializedValue);

        string DynamicSerialize(object value);
    }

    [RequiresConstructorContract]
    public interface IConfigStringSerializer<TValueType> : IConfigStringSerializer
    {
        TValueType Deserialize(string serializedValue);

        string Serialize(TValueType value);
    }
}