using CVB.NET.Abstractions.Ioc;
using CVB.NET.Rewriting.Compiler.Helpers;

namespace CVB.NET.Rewriting.Compiler.Ioc.Bootstrap
{
    public interface ICompilerDomainBootstrapper
    {
        IDependencyService CreateDependencyService();
        void Bootstrap();
    }
}