using System;
using System.Collections.Generic;

using CVB.NET.Rewriting.Compiler.Configuration.Models.Attributes;
using CVB.NET.Rewriting.Compiler.Configuration.Models.CompilationUnit.Task;

namespace CVB.NET.Rewriting.Compiler.CompilationUnits.Tasks.Cecil
{
    [ConfigurationInterfaceImplementation(typeof(CecilScopedTransformationConfiguration))]
    public interface ICecilScopedTransformationConfiguration : ICompilationTaskConfiguration
    {
        IEnumerable<Type> TargetTransformationTypes { get; }

        string TargetTag { get; }
    }

    [Serializable]
    public class CecilScopedTransformationConfiguration : CompilationTaskConfigurationElement, ICecilScopedTransformationConfiguration
    {
        public IEnumerable<Type> TargetTransformationTypes { get; }

        public string TargetTag { get; }
    }
}