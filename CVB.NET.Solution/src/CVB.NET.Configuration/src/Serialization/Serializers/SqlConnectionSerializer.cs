namespace CVB.NET.Configuration.Serialization.Serializers
{
    using System.Data.SqlClient;
    using Base;

    public class SqlConnectionSerializer : ConfigStringSerializerBase<SqlConnection>
    {
        public override SqlConnection Deserialize(string serializedValue)
        {
            return new SqlConnection(serializedValue);
        }

        public override string Serialize(SqlConnection value)
        {
            return value.ConnectionString;
        }
    }
}