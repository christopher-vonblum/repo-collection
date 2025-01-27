using System.Collections.Generic;
using CVB.NET.Rewriting.Compiler.Ioc.Bootstrap;
using CVB.NET.Rewriting.Compiler.Services;

namespace CVB.NET.Rewriting.Compiler.CompilationUnit.Factory.Model
{
    public interface ICompilationUnitFactoryConfiguration
    {
        IEnumerable<ICompilationUnitInitializer> Initializers { get; }
    }
}
