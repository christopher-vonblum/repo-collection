namespace CVB.NET.DataAccess.Exception
{
    public class DeleteRecordException : CrudRepositoryException
    {
        public DeleteRecordException(System.Exception innerException) : base(innerException)
        {
        }
    }
}