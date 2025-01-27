namespace CVB.NET.Rewriting.Compiler.Configuration.Exception
{
    public class CompilationTaskNotFoundException : CompilationUnitNotFoundException
    {
        public CompilationTaskNotFoundException(string name)
            : base(name)
        {
        }
    }
}