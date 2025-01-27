namespace CVB.NET.DataAccess.Sql.Exception
{
    using System.Data.SqlClient;
    using DataAccess.Exception;

    public class CreateRootIdentityException : CrudRepositoryException
    {
        public CreateRootIdentityException(SqlException sqlException) : base(sqlException)
        {
        }
    }
}