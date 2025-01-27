namespace CVB.NET.Reflection.Aspects.ParameterValidation
{
    using System;
    using Exceptions.Reflection;
    using PostSharp.Aspects;
    using PostSharp.Patterns.Contracts;
    using PostSharp.Reflection;

    public class IsAssignableToAttribute : LocationContractAttribute, ILocationValidationAspect<Type>
    {
        private Type AssignTargetType { get; }

        public IsAssignableToAttribute(Type assignTargetType)
        {
            AssignTargetType = assignTargetType;
        }

        public System.Exception ValidateValue(Type value, string locationName, LocationKind locationKind)
        {
            if (value == null)
            {
                return new ArgumentNullException(locationName);
            }

            if (AssignTargetType.IsAssignableFrom(value))
            {
                return null;
            }

            return new TypeIsNotAssignableToException(value);
        }
    }
}