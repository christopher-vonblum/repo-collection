namespace CVB.NET.Aspects.ParameterValidation.FileSystem
{
    using System;
    using System.IO;
    using PostSharp.Aspects;
    using PostSharp.Patterns.Contracts;
    using PostSharp.Reflection;

    public class PathExists : LocationContractAttribute, ILocationValidationAspect<string>
    {
        public Exception ValidateValue(string value, string locationName, LocationKind locationKind)
        {
            if (Directory.Exists(value))
            {
                return null;
            }

            if (File.Exists(value))
            {
                return null;
            }

            try
            {
                Path.GetFullPath(value);
                return new DirectoryNotFoundException(value);
            }
            catch
            {
                return new FileNotFoundException(value);
            }
        }
    }
}