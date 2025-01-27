using System;
using CVB.NET.Reflection.Caching.Cached;
using CVB.NET.Rewriting.Compiler.Argument;

namespace CVB.NET.Rewriting.Compiler.BuildIntegration.MsBuild
{
    [Serializable]
    public class MsBuildArgs : IPredefinedBuildArgs
    {
        public string SolutionFile { get; set; }
        public string ProjectFile { get; set; }
        public string Configuration { get; set; }
        public string Platform { get; set; }

        public string CompilerDomainBoostrapperType { get; set; } = typeof(MsBuildCompilerDomainBootstrapper).AssemblyQualifiedName;
        CachedType IPredefinedBuildArgs.CompilerDomainBootstrapperType => Type.GetType(this.CompilerDomainBoostrapperType);
        public bool AttachDebugger { get; set; }
        
    }
}