using System.Collections.Generic;

namespace CVB.NET.Rewriting.CompilerArgs
{
    public class Args : IArgs
    {
        /// <inheritdoc />
        public string CompilerInstallationPath { get; set; }
        
        /// <inheritdoc />
        public string CompilerResultDocumentPath { get; set; }
        
        /// <inheritdoc />
        public string IntermediateOutputPath { get; set; }
        
        /// <inheritdoc />
        public string IntermediateAssemblyPath { get; set; }
        
        /// <inheritdoc />
        public List<string> RewritingMiddlewareTypes { get; set; }
    }
}