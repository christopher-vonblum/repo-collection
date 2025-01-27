namespace CVB.NET.Reflection.Aspects.ParameterValidation
{
    using System;
    using Exceptions.Reflection;
    using PostSharp.Aspects;
    using PostSharp.Patterns.Contracts;
    using PostSharp.Reflection;

    public class IsSubclassOfAttribute : LocationContractAttribute, ILocationValidationAspect<Type>
    {
        private Type SuperClass { get; }

        public IsSubclassOfAttribute(Type superClass)
        {
            SuperClass = superClass;
        }

        public System.Exception ValidateValue(Type value, string locationName, LocationKind locationKind)
        {
            if (value == null)
            {
                return new ArgumentNullException(locationName);
            }

            if (value.IsSubclassOf(SuperClass))
            {
                return null;
            }

            return new TypeIsNoInterfaceException(value);
        }
    }
}