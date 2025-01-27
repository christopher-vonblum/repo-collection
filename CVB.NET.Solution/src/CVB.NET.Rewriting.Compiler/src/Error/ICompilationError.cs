namespace CVB.NET.Rewriting.Compiler.Error
{
    public interface ICompilationError
    {
        DocumentReference[] Locations { get; }
        string Category { get; }
        string Message { get; }
        string Code { get; }
    }
}