using System;
using System.Collections.Generic;

namespace CVB.NET.Rewriting.CompilerArgs
{
    public interface IArgs
    {
        /// <summary>
        /// Location of CVB.NET.Rewriting.Compiler executable and dependencies of the compiler.
        /// </summary>
        string CompilerInstallationPath { get; set; }
        
        /// <summary>
        /// Path of json documents that the compiler outputs.
        /// </summary>
        string CompilerResultDocumentPath { get; set; }
        
        /// <summary>
        /// Path of the obj folder.
        /// </summary>
        string IntermediateOutputPath { get; set; }
        
        /// <summary>
        /// Path of the assembly that is subject to IL rewriting.
        /// </summary>
        string IntermediateAssemblyPath { get; set; }

        /// <summary>
        /// AssemblyQualifiedName´s of the middlewares that shall process the intermediate-assembly.
        /// </summary>
        List<string> RewritingMiddlewareTypes { get; set; }
        
        /// <summary>
        /// Directory of the msbuild-solution-file.
        /// </summary>
        string SolutionDirectory { get; set; }
        
        string MiddlewareLoadPath { get; set; }
    }
}
