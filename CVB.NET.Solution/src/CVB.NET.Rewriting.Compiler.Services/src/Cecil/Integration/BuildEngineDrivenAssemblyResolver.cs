using System.Linq;
using CVB.NET.Rewriting.Compiler.BuildIntegration;
using Mono.Cecil;

namespace CVB.NET.Rewriting.Compiler.Services.Cecil.Integration
{
    public class BuildEngineDrivenAssemblyResolver : DefaultAssemblyResolver
    {
        public BuildEngineDrivenAssemblyResolver(IBuildEngine buildEngine)
        {
            buildEngine.GetFileResolverSearchPaths().ToList().ForEach(path => this.AddSearchDirectory(path));
        }
    }
}
