namespace CVB.NET.DataAccess.Sql.PrimitiveSerialization
{
    using System;

    public class TypeAsPrimitiveSerializer : PrimitiveSerializerBase<Type>
    {
        public TypeAsPrimitiveSerializer() : base("varchar(MAX)")
        {
        }

        public override object Serialize(Type value)
        {
            throw new NotImplementedException();
        }

        public override Type Deserialize(object sqlValue)
        {
            throw new NotImplementedException();
        }
    }
}