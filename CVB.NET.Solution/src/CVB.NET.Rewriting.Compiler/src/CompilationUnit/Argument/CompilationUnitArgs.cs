using System.IO;
using System.Reflection;

namespace CVB.NET.Rewriting.Compiler.CompilationUnit.Argument
{
    using System;

    public interface IInternalCompilationUnitArgs : ICompilationUnitArgs
    {
        string IntermediatePath { get; set; }
        string IntermediateAssembly { get; set; }
    }

    [Serializable]
    public class CompilationUnitArgs : IInternalCompilationUnitArgs
    {
        public string PreTransformationPath { get; set; }

        public string TransformationOutputPath { get; set; }

        public string TransformationOutputAssembly { get; set; }
        public AssemblyName AssemblyName { get; set; }
        public string IntermediatePath { get; set; }
        public string IntermediateAssembly { get; set; }
        public void CreatePaths()
        {
            if(Directory.Exists(PreTransformationPath)) Directory.Delete(PreTransformationPath, true);
            if(Directory.Exists(TransformationOutputPath)) Directory.Delete(TransformationOutputPath, true);
            Directory.CreateDirectory(TransformationOutputPath);
            Directory.CreateDirectory(PreTransformationPath);
        }
    }
}