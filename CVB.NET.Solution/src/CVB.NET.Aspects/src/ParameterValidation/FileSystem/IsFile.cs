namespace CVB.NET.Aspects.ParameterValidation.FileSystem
{
    using System;
    using PostSharp.Aspects;
    using PostSharp.Patterns.Contracts;
    using PostSharp.Reflection;

    public class IsFile : LocationContractAttribute, ILocationValidationAspect<string>
    {
        public Exception ValidateValue(string value, string locationName, LocationKind locationKind)
        {
            throw new NotImplementedException();
        }
    }
}