using System.Collections.Generic;
using CVB.NET.Rewriting.Compiler.Ioc.Bootstrap;
using CVB.NET.Rewriting.Compiler.Services;

namespace CVB.NET.Rewriting.Compiler.CompilationUnit.Factory.Model
{
    public class CompilationUnitFactoryConfiguration : ICompilationUnitFactoryConfiguration
    {
        public IEnumerable<ICompilationUnitInitializer> Initializers { get; set; }
    }
}