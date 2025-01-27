namespace CVB.NET.Ioc.Exception
{
    using System;

    public class NoImplementationFoundException : System.Exception
    {
        public NoImplementationFoundException(Type serviceInterface)
            : base("Missing implementation for \"" + serviceInterface.FullName + "\".")
        {
        }
    }
}