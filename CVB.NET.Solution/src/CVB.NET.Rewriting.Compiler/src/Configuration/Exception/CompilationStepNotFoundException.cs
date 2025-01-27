namespace CVB.NET.Rewriting.Compiler.Configuration.Exception
{
    public class CompilationStepNotFoundException : CompilationUnitNotFoundException
    {
        public CompilationStepNotFoundException(string name)
            : base(name)
        {
        }
    }
}