namespace CVB.NET.DataAccess.Sql.T4Queries.Model.Base
{
    using System.Data.SqlClient;

    public class DatabaseScopedQueryDescription
    {
        public SqlConnection Connection { get; set; }

        public string Database
        {
            get
            {
                if (Connection == null)
                {
                    return database;
                }

                return database ?? Connection.Database;
            }
            set { database = value; }
        }

        private string database;

        public DatabaseScopedQueryDescription(SqlConnection connection = null, string database = null)
        {
            Connection = connection;

            this.database = database;
        }

        public DatabaseScopedQueryDescription(string database = null, SqlConnection connection = null) : this(connection, database)
        {
        }
    }
}