namespace CVB.NET.DataAccess.Sql.Exception
{
    using System.Data.SqlClient;
    using DataAccess.Exception;

    public class CreateTypeIdentityException : CrudRepositoryException
    {
        public CreateTypeIdentityException(SqlException sqlException) : base(sqlException)
        {
        }
    }
}