namespace CVB.NET.Reflection.Aspects.ParameterValidation
{
    using System;

    public class TypeDoesNotImplementInterfaceException : Exception
    {
        public TypeDoesNotImplementInterfaceException(Type value)
        {
            throw new NotImplementedException();
        }
    }
}