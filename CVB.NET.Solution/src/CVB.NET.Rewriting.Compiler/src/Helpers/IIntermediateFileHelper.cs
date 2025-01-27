namespace CVB.NET.Rewriting.Compiler.Helpers
{
    public interface IIntermediateFileHelper
    {
        void CopyToPreTransformationPath(string file);

        bool IsCopiedToPreTransformationPath(string file);
        void EnsureCopiedToPreTransformationPath(string file);

        string GetPreTransformationFilePath(string file);
        string GetTransformationFilePath(string file);
    }
}