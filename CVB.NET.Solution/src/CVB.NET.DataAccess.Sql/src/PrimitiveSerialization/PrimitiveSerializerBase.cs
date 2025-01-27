namespace CVB.NET.DataAccess.Sql.PrimitiveSerialization
{
    public abstract class PrimitiveSerializerBase<T> : IPrimitiveSerializerBase<T>
    {
        public PrimitiveSerializerBase(string sqlDataType)
        {
            SqlDataType = sqlDataType;
        }

        public string SqlDataType { get; }

        public abstract object Serialize(T value);

        public abstract T Deserialize(object sqlValue);
    }
}