namespace CVB.NET.DataAccess.Sql.Attributes
{
    using System;

    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Property)]
    public class UsePrimitiveSerializerAttribute : Attribute
    {
        public Type PrimitiveSerializerType { get; }

        public UsePrimitiveSerializerAttribute(Type primitiveSerializerType)
        {
            PrimitiveSerializerType = primitiveSerializerType;
        }
    }
}