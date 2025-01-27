namespace CVB.NET.Exceptions.Reflection
{
    public class TypeIsNoInterfaceException : System.Exception
    {
        public System.Type NonInterfaceType { get; }

        public TypeIsNoInterfaceException(System.Type nonInterfaceType) : base("Type " + nonInterfaceType.FullName + " is not a interface.")
        {
            NonInterfaceType = nonInterfaceType;
        }
    }
}