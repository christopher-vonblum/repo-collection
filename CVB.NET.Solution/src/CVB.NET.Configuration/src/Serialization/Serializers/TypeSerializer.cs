namespace CVB.NET.Configuration.Serialization.Serializers
{
    using System;
    using Base;

    public class TypeSerializer : ConfigStringSerializerBase<Type>
    {
        public override Type Deserialize(string serializedValue)
        {
            return Type.GetType(serializedValue); //;new GenericType(null, serializedValue).GetGenericType();
        }

        public override string Serialize(Type value)
        {
            return value.AssemblyQualifiedName;
        }
    }
}