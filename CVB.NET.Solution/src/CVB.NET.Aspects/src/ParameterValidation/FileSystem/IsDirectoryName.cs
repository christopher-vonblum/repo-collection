namespace CVB.NET.Aspects.ParameterValidation.FileSystem
{
    using System;
    using System.IO;
    using PostSharp.Aspects;
    using PostSharp.Patterns.Contracts;
    using PostSharp.Reflection;

    public class IsDirectoryName : LocationContractAttribute, ILocationValidationAspect<string>
    {
        public Exception ValidateValue(string value, string locationName, LocationKind locationKind)
        {
            bool validPathName = value.IndexOfAny(Path.GetInvalidPathChars()) >= 0;

            if (validPathName)
            {
                return null;
            }

            return new FileNotFoundException(value);
        }
    }
}