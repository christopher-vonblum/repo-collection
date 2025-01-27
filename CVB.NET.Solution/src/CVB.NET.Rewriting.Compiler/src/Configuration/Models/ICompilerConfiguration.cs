using CVB.NET.Rewriting.Compiler.Configuration.Models.CompilationUnit.Task;
using CVB.NET.Rewriting.Compiler.Configuration.Models.ServiceAssembly;

namespace CVB.NET.Rewriting.Compiler.Configuration.Models
{
    using System.Collections.Generic;
    using CompilationUnit.Step;

    public interface ICompilerConfiguration
    {
        IEnumerable<ICompilationStepConfiguration> CompilationSteps { get; }
        IEnumerable<ICompilationTaskConfiguration> CompilationTasks { get; }
        IEnumerable<IServiceAssemblyReferenceConfiguration> ServiceAssemblies { get; }
    }
}