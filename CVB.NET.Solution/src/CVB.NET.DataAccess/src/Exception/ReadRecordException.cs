namespace CVB.NET.DataAccess.Exception
{
    public class ReadRecordException : CrudRepositoryException
    {
        public ReadRecordException(System.Exception innerException) : base(innerException)
        {
        }
    }
}