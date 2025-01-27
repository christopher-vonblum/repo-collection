namespace CVB.NET.Rewriting.Compiler.CompilationUnit
{
    using System;
    using System.Linq;
    using Argument;
    using Configuration.Models.CompilationUnit;
    using Error;
    using Result;

    public abstract class CompilationUnitBase<TUnitConfigurationInterface> : ICompilationUnit
        where TUnitConfigurationInterface : class, ICompilationUnitConfiguration
    {
        public TUnitConfigurationInterface Configuration { get; private set; }

        ICompilationUnitConfiguration ICompilationUnit.Configuration => Configuration;

        public void Configure(ICompilationUnitConfiguration unitConfiguration)
        {
            Configuration = (TUnitConfigurationInterface)unitConfiguration;
        }

        public ICompilationUnitResult Invoke(ICompilationUnitArgs args)
        {
            try
            {
                return Execute(args);
            }

            catch (Exception ex)
            {
                return CreateResult(CompilationError.FromException(ex));
            }
        }

        protected ICompilationUnitResult CreateResult(bool buildSucceeded = true)
        {
            return new CompilationUnitResult
            {
                BuildSucceeded = buildSucceeded,
                ChildResults = new ICompilationUnitResult[0],
                CompilationErrors = new ICompilationError[0]
            };
        }

        protected ICompilationUnitResult CreateResult(params ICompilationError[] errors)
        {
            return new CompilationUnitResult
            {
                BuildSucceeded = !errors.Any(),
                ChildResults = new ICompilationUnitResult[0],
                CompilationErrors = errors
            };
        }

        public abstract ICompilationUnitResult Execute(ICompilationUnitArgs args);
    }
}