namespace CVB.NET.DataAccess.Sql.PrimitiveSerialization
{
    using System;

    public class TypeSerializer : PrimitiveSerializerBase<Type>
    {
        public TypeSerializer() : base("varchar(MAX)")
        {
        }

        public override object Serialize(Type value)
        {
            return value.AssemblyQualifiedName;
        }

        public override Type Deserialize(object sqlValue)
        {
            return Type.GetType((string) sqlValue);
        }
    }
}