namespace CVB.NET.TextTemplating.Hosted.Utils
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Microsoft.VisualStudio.TextTemplating;
    using NET.Utils.String;
    using PostSharp.Patterns.Contracts;
    using Reflection.Caching.Cached;

    public static class TextTransformationExtensions
    {
        private static Engine Engine = new Engine();

        public static void Include([NotNull] this TextTransformation textTransformation, [NotEmpty] string templatePath, IDictionary<string, object> namedArguments = null)
        {
            CachedType textTransformationType = textTransformation.GetType();

            CachedPropertyInfo hostPropertyInfo = textTransformationType
                .Properties
                .Single(prop => "Host".Equals(prop.InnerReflectionInfo.Name));

            ITextTemplatingEngineHost transformationHost = (ITextTemplatingEngineHost) hostPropertyInfo
                .InnerReflectionInfo
                .GetValue(textTransformation);

            IDictionary<string, object> sessionArguments = namedArguments ?? textTransformation.Session;

            sessionArguments["PhysicalRootPath"] = textTransformation.Session["PhysicalRootPath"];

            string physicalTemplatePath = PathUtil.MapPhysicalPath((string) sessionArguments["PhysicalRootPath"], templatePath);

            textTransformation.Write(Engine.ProcessTemplate(File.ReadAllText(physicalTemplatePath), transformationHost));
        }
    }
}