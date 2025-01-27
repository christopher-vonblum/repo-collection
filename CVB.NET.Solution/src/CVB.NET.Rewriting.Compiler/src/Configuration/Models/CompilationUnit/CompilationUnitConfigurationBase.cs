namespace CVB.NET.Rewriting.Compiler.Configuration.Models.CompilationUnit
{
    using System;
    using System.Collections.Generic;

    using NET.Configuration.Attributes;
    using NET.Configuration.Base;
    using Reflection.Caching.Cached;
    using Rewriting.Compiler.CompilationUnit;

    [Serializable]
    public class CompilationUnitConfigurationBase : ConfigurationElementBase, ICompilationUnitConfiguration
    {
        [IdentifierProperty]
        public string Name { get; }

        public virtual Type ImplementationType { get; }

        public bool IsolateUnit { get; }

        public string[] PreExecutionTasks { get; }

        public bool ExecutePreExecutionTasksOrdered { get; } = true;

        public string[] PostExecutionTasks { get; }
        public bool ExecutePostExecutionTasksOrdered { get; } = true;

        public Dictionary<string, string> Parameters { get; }
        IReadOnlyDictionary<string, string> ICompilationUnitConfiguration.Parameters => Parameters;
        
        private Type configurationImplementationType;

        Type ICompilationUnitConfiguration.ConfigurationImplementationType => configurationImplementationType ?? (configurationImplementationType = GetConfigurationImplementationType(ImplementationType));
        private Type GetConfigurationImplementationType(CachedType search)
        {
            if (search.InnerReflectionInfo == typeof(object))
            {
                return null;
            }

            if (search.InnerReflectionInfo.IsGenericType && search.GenericTypeDefinition.InnerReflectionInfo == typeof(CompilationUnitBase<>))
            {
                return search.GenericTypeArguments[0];
            }

            return GetConfigurationImplementationType(search.InnerReflectionInfo.BaseType);
        }

        public override object GetProperty(string propertyName)
        {
            return Parameters[propertyName];
        }
    }
}