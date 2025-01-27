namespace CVB.NET.Rewriting.Compiler.Configuration.Exception
{
    public class CompilationUnitNotFoundException : System.Exception
    {
        public string Name { get; set; }

        public CompilationUnitNotFoundException(string name)
        {
            this.Name = name;
        }
    }
}