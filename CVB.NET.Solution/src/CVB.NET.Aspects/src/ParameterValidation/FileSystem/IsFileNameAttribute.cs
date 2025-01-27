namespace CVB.NET.Aspects.ParameterValidation.FileSystem
{
    using System;
    using System.IO;
    using PostSharp.Aspects;
    using PostSharp.Patterns.Contracts;
    using PostSharp.Reflection;

    public class IsFileNameAttribute : LocationContractAttribute, ILocationValidationAspect<string>
    {
        public Exception ValidateValue(string value, string locationName, LocationKind locationKind)
        {
            bool validFileName = value.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0;

            if (validFileName)
            {
                return null;
            }

            return new FormatException(value);
        }
    }
}