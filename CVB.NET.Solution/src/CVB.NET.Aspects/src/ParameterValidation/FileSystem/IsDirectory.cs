namespace CVB.NET.Aspects.ParameterValidation.FileSystem
{
    using System;
    using System.IO;
    using PostSharp.Aspects;
    using PostSharp.Patterns.Contracts;
    using PostSharp.Reflection;

    public class IsDirectory : LocationContractAttribute, ILocationValidationAspect<string>
    {
        public Exception ValidateValue(string value, string locationName, LocationKind locationKind)
        {
            try
            {
                Path.GetFullPath(value);
                return null;
            }
            catch
            {
                return new FormatException(value);
            }
        }
    }
}