namespace CVB.NET.Reflection.Aspects.ParameterValidation
{
    using Exceptions.Reflection;
    using PostSharp.Aspects;
    using PostSharp.Patterns.Contracts;
    using PostSharp.Reflection;

    public class IsStaticAttribute : LocationContractAttribute, ILocationValidationAspect<System.Type>
    {
        public System.Exception ValidateValue(System.Type value, string locationName, LocationKind locationKind)
        {
            if (value.IsAbstract && value.IsSealed)
            {
                return null;
            }

            return new TypeIsNotStaticException(value);
        }
    }
}