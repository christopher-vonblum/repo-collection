using CVB.NET.Rewriting.Compiler.CompilationUnit.Argument;
using CVB.NET.Rewriting.Compiler.CompilationUnit.Result;
using CVB.NET.Rewriting.Compiler.CompilationUnit.Task;
using CVB.NET.Rewriting.Compiler.CompilationUnits.Tasks.Cecil;
using CVB.NET.Rewriting.Compiler.Helpers;
using CVB.NET.Rewriting.Compiler.Services.Interfaces.Cecil;

using Mono.Cecil;

namespace CVB.NET.Rewriting.Compiler.CompilationUnits.src.Tasks.Cecil
{
    public class CacheEmbeddingTask : CompilationTaskBase<ICecilScopedTransformationConfiguration>
    {
        private readonly IAssemblyDependencyResolver assemblyHelper;

        private readonly ICecilAssemblyTransformationContext transformationContext;

        public CacheEmbeddingTask(IAssemblyDependencyResolver assemblyHelper, ICecilAssemblyTransformationContext transformationContext)
        {
            this.assemblyHelper = assemblyHelper;
            this.transformationContext = transformationContext;
        }

        public override ICompilationUnitResult Execute(ICompilationUnitArgs args)
        {
            AssemblyDefinition transformationAssembly = this.transformationContext.GetTransformationAssembly(args.AssemblyName);

            transformationAssembly.MainModule.Resources.Add(new EmbeddedResource(this.Configuration.TargetTag, ManifestResourceAttributes.Public, ));
        }
    }
}
