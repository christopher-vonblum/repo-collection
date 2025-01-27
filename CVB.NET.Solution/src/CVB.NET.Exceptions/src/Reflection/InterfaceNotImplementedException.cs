namespace CVB.NET.Exceptions.Reflection
{
    using System;

    public class InterfaceNotImplementedException : Exception
    {
        public InterfaceNotImplementedException(Type type, Type @interface) : base(type.FullName + " does not implement interface " + @interface.FullName + ".")
        {
        }
    }
}