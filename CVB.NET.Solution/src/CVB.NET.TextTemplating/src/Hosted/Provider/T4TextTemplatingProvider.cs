namespace CVB.NET.TextTemplating.Hosted.Provider
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using Aspects.Patterns.AutoDictionaryWrapper;
    using Base;
    using Host;
    using Microsoft.VisualStudio.TextTemplating;
    using NET.Utils.String;
    using PostSharp.Patterns.Contracts;
    using Reflection.Caching.Cached;

    public class T4TextTemplatingProvider : ITextTemplatingProvider
    {
        private static ConcurrentDictionary<string, T4TemplateEngineHost> EngineHosts =
            new ConcurrentDictionary<string, T4TemplateEngineHost>();

        private string PhysicalRootPath { get; }

        private Engine Engine = new Engine();

        public T4TextTemplatingProvider([NotEmpty] string physicalRootPath)
        {
            PhysicalRootPath = physicalRootPath;
        }

        public string ProcessTemplate(string templateFilePath, object transformationModel)
        {
            if (transformationModel is IAutoDictionaryWrapper)
            {
                return ProcessTemplate(templateFilePath, ((IAutoDictionaryWrapper) transformationModel).WrappedDictionary);
            }

            CachedType transformationModelType = transformationModel.GetType();

            Dictionary<string, object> arguments = new Dictionary<string, object>();

            foreach (PropertyInfo propertyInfo in transformationModelType.Properties)
            {
                arguments.Add(propertyInfo.Name, propertyInfo.GetValue(transformationModel));
            }

            return ProcessTemplate(templateFilePath, arguments);
        }

        public string ProcessTemplate(string templateFilePath, IDictionary<string, object> arguments)
        {
            string physicalTemplatePath = PathUtil.MapPhysicalPath(PhysicalRootPath, templateFilePath);

            T4TemplateEngineHost templateFileHost = EngineHosts.GetOrAdd(physicalTemplatePath, (string templatePath) => new T4TemplateEngineHost(templateFilePath));

            string templateText = File.ReadAllText(templateFileHost.TemplateFile);

            templateFileHost.CreateSession();

            templateFileHost.Session["PhysicalRootPath"] = PhysicalRootPath;

            foreach (KeyValuePair<string, object> argument in arguments)
            {
                templateFileHost.Session[argument.Key] = argument.Value;
            }

            return Engine.ProcessTemplate(templateText, templateFileHost);
        }
    }
}