namespace CVB.NET.DataAccess.Sql.Exception
{
    using System.Data.SqlClient;
    using DataAccess.Exception;

    public class CreateForeignKeyConstraintsException : CrudRepositoryException
    {
        public CreateForeignKeyConstraintsException(SqlException sqlException) : base(sqlException)
        {
        }
    }
}