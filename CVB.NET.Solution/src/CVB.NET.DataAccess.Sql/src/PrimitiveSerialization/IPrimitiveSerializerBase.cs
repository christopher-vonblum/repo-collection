namespace CVB.NET.DataAccess.Sql.PrimitiveSerialization
{
    public interface IPrimitiveSerializerBase<T>
    {
        string SqlDataType { get; }

        T Deserialize(object sqlValue);
        object Serialize(T value);
    }
}