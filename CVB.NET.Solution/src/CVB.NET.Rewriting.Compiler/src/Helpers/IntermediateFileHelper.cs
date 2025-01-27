using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.AccessControl;
using CVB.NET.Rewriting.Compiler.CompilationUnit.Argument;

namespace CVB.NET.Rewriting.Compiler.Helpers
{
    public class IntermediateFileHelper : IIntermediateFileHelper
    {
        private readonly IInternalCompilationUnitArgs arguments;

        public IntermediateFileHelper(IInternalCompilationUnitArgs arguments)
        {
            this.arguments = arguments;

            Directory.GetFiles(arguments.IntermediatePath).ToList().ForEach(f =>
                                                                            {
                                                                                try
                                                                                {
                                                                                    CopyToPreTransformationPath(f);
                                                                                }
                                                                                catch { }
                                                                            });
        }

        public void CopyToPreTransformationPath(string file)
        {
            string fileName = Path.GetFileName(file);

            string intermediateSource = this.arguments.IntermediatePath + fileName;

            File.Copy(intermediateSource, GetPreTransformationFilePath(file), true);
        }

        public bool IsCopiedToPreTransformationPath(string file)
        {
            return File.Exists(GetPreTransformationFilePath(file));
        }

        public void EnsureCopiedToPreTransformationPath(string file)
        {
            if (this.IsCopiedToPreTransformationPath(file))
            {
                return;
            }

            this.CopyToPreTransformationPath(file);
        }

        public string GetPreTransformationFilePath(string file)
        {
            return this.arguments.PreTransformationPath + Path.GetFileName(file);
        }

        public string GetTransformationFilePath(string file)
        {
            return this.arguments.TransformationOutputPath + Path.GetFileName(file);
        }
    }
}