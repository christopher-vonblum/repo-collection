namespace CVB.NET.DataAccess.Exception
{
    public class UpdateRecordException : CrudRepositoryException
    {
        public UpdateRecordException(System.Exception innerException) : base(innerException)
        {
        }
    }
}