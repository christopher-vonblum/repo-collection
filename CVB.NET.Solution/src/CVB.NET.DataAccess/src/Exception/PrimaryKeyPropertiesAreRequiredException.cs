namespace CVB.NET.DataAccess.Exception
{
    public class PrimaryKeyPropertiesAreRequiredException : System.Exception
    {
        public string MissingPrimaryKeyPropertyName { get; }

        public PrimaryKeyPropertiesAreRequiredException(string name) : base(name)
        {
            MissingPrimaryKeyPropertyName = name;
        }
    }
}