namespace CVB.NET.Reflection.Aspects.ParameterValidation
{
    using System;
    using System.Linq;
    using PostSharp.Aspects;
    using PostSharp.Patterns.Contracts;
    using PostSharp.Reflection;

    public class ImplementsInterfaceAttribute : LocationContractAttribute, ILocationValidationAspect<Type>
    {
        private Type InterfaceType { get; }

        public ImplementsInterfaceAttribute(Type interfaceType)
        {
            InterfaceType = interfaceType;
        }

        public Exception ValidateValue(Type value, string locationName, LocationKind locationKind)
        {
            if (value == null)
            {
                return new ArgumentNullException(locationName);
            }

            if (value.IsClass && value.GetInterfaces().SingleOrDefault(iface => iface == InterfaceType) != null)
            {
                return null;
            }

            return new TypeDoesNotImplementInterfaceException(value);
        }
    }
}