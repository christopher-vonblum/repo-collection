namespace CVB.NET.DataAccess.Exception
{
    using System.Data.SqlClient;
    using PostSharp.Patterns.Contracts;

    public class CrudRepositoryException : System.Exception
    {
        public SqlCommand Command { get; set; }

        public CrudRepositoryException([NotNull] System.Exception innerException) : base(innerException.Message, innerException)
        {
        }
    }
}