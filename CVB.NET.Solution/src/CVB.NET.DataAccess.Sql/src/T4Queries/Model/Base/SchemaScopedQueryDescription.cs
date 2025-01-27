namespace CVB.NET.DataAccess.Sql.T4Queries.Model.Base
{
    using System.Data.SqlClient;

    public class SchemaScopedQueryDescription : DatabaseScopedQueryDescription
    {
        public string Schema { get; set; }

        public SchemaScopedQueryDescription(string schema = null, string database = null, SqlConnection connection = null) : base(database, connection)
        {
            Schema = schema;
        }
    }
}