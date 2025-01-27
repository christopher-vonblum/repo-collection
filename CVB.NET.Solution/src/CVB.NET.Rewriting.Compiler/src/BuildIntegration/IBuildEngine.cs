using System.Collections.Generic;
using CVB.NET.Rewriting.Compiler.CompilationUnit.Argument;
using CVB.NET.Rewriting.Compiler.Configuration.Models.CompilationUnit;

namespace CVB.NET.Rewriting.Compiler.BuildIntegration
{
    public interface IBuildEngine
    {
        /// <summary>
        /// Provides paths to search for files that need to be found while compiling.
        /// These are typically paths to look for assemblies referenced from targeted assembly, locations of external references or other files.
        /// For MsBuild all obj/$(Configuration) folders will be included here.
        /// </summary>
        /// <returns></returns>
        IEnumerable<string> GetFileResolverSearchPaths();

        IInternalCompilationUnitArgs MakeExecutionUnitArgs(ICompilationUnitConfiguration unitConfiguration);
    }
}