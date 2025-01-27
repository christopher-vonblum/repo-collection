using System;
using System.Runtime.Serialization;

namespace CVB.NET.Rewriting.Compiler.Configuration.Models
{
    using System.Collections.Generic;
    using CompilationUnit.Step;
    using CompilationUnit.Task;
    using NET.Configuration.Base;
    using ServiceAssembly;

    [Serializable]
    public class CompilerConfigurationSection : ConfigurationSectionBase, ICompilerConfiguration
    {
        public CompilerConfigurationSection()
        {

        }

        protected CompilerConfigurationSection(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public ConfigurationElementCollection<CompilationStepConfigurationElement> CompilationSteps { get; }
        public ConfigurationElementCollection<CompilationTaskConfigurationElement> CompilationTasks { get; }
        public ConfigurationElementCollection<ServiceAssemblyReferenceConfigurationElement> ServiceAssemblies { get; } 

        IEnumerable<ICompilationStepConfiguration> ICompilerConfiguration.CompilationSteps => CompilationSteps;
        
        IEnumerable<ICompilationTaskConfiguration> ICompilerConfiguration.CompilationTasks => CompilationTasks;

        IEnumerable<IServiceAssemblyReferenceConfiguration> ICompilerConfiguration.ServiceAssemblies => ServiceAssemblies;
    }
}