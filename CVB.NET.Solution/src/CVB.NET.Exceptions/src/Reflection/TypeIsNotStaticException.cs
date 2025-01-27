namespace CVB.NET.Exceptions.Reflection
{
    public class TypeIsNotStaticException : System.Exception
    {
        private System.Type NonStaticType { get; }

        public TypeIsNotStaticException(System.Type value) : base("Type " + value.FullName + " is not static.")
        {
            NonStaticType = value;
        }
    }
}