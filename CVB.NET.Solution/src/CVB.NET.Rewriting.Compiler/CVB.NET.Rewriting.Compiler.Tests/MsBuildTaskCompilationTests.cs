using System;

using CVB.NET.Rewriting.Compiler.Result;

using NUnit.Framework;

namespace CVB.NET.Rewriting.Compiler.Tests
{
    using BuildIntegration.MsBuild;
    using Ioc;

    [TestFixture]
    public class MsBuildTaskCompilationTests
    {
        [TestCase]
        public void TestMethod1()
        {
            ICompilationResult result = CompilationApi.Compile(new MsBuildArgs
            {
                SolutionFile = @"C:\Users\IBM_ADMIN\Documents\Privat\CVB.NET.Solution\src\CVB.NET.Rewriting.Compiler\CVB.NET.Rewriting.Compiler.Tests\TestSolutions\TestSolution1.sln",
                ProjectFile = @"C:\Users\IBM_ADMIN\Documents\Privat\CVB.NET.Solution\src\CVB.NET.Rewriting.Compiler\CVB.NET.Rewriting.Compiler.Tests\TestSolutions\ExampleLib1\ExampleLib1.csproj",
                Configuration = "Debug",
                Platform = "Any CPU"
            },
            @"C:\Users\IBM_ADMIN\Documents\Privat\CVB.NET.Solution\src\CVB.NET.Rewriting.Compiler\CVB.NET.Rewriting.Compiler.Tests\bin\Debug\TestCompilation.config",
            "customCompilation");

            Assert.IsTrue(result.CompilationSucceeded);
        }
    }
}