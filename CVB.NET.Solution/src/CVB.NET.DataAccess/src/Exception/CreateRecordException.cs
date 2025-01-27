namespace CVB.NET.DataAccess.Exception
{
    public class CreateRecordException : CrudRepositoryException
    {
        public CreateRecordException(System.Exception innerException) : base(innerException)
        {
        }
    }
}