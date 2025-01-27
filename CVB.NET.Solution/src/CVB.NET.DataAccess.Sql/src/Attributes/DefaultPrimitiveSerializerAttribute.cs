namespace CVB.NET.DataAccess.Sql.Attributes
{
    using System;
    using PrimitiveSerialization;
    using Reflection.Aspects.ParameterValidation;

    public class DefaultPrimitiveSerializerAttribute : Attribute
    {
        public Type PrimitiveSerializerType { get; }

        public DefaultPrimitiveSerializerAttribute([ImplementsInterface(typeof (IPrimitiveSerializerBase<>))] Type primitiveSerializerType)
        {
            PrimitiveSerializerType = primitiveSerializerType;
        }
    }
}