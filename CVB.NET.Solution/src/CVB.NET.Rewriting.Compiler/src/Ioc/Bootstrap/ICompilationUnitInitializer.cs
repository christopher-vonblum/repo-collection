using CVB.NET.Abstractions.Ioc;
using CVB.NET.Rewriting.Compiler.CompilationUnit.Argument;

namespace CVB.NET.Rewriting.Compiler.Ioc.Bootstrap
{
    public interface ICompilationUnitInitializer
    {
        void InitializeUnit(IUnitInitializationArgs initializationArgs);
    }

    public interface IUnitInitializationArgs
    {
        IDependencyInstaller DependencyInstaller { get; }
        ICompilationUnitArgs CompilationUnitArgs { get; }
        IDependencyResolver DependencyResolver { get; set; }
    }

    public class UnitInitializationArgs : IUnitInitializationArgs
    {
        public IDependencyInstaller DependencyInstaller { get; set; }
        public ICompilationUnitArgs CompilationUnitArgs { get; set; }
        public IDependencyResolver DependencyResolver { get; set; }
    }
}