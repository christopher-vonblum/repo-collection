namespace CVB.NET.DataAccess.Sql.Attributes
{
    public class CustomColumnNameAttribute
    {
        public string ColummnName { get; private set; }

        public CustomColumnNameAttribute(string columnName)
        {
            ColummnName = columnName;
        }
    }
}