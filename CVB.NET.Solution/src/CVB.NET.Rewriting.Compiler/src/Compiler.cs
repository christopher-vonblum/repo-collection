using CVB.NET.Abstractions.Ioc;
using CVB.NET.Rewriting.Compiler.BuildIntegration;
using CVB.NET.Rewriting.Compiler.CompilationUnit.Driver;
using CVB.NET.Rewriting.Compiler.Configuration;

namespace CVB.NET.Rewriting.Compiler
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Argument;
    using CompilationUnit.Argument;
    using CompilationUnit.Result;
    using Configuration.Models.CompilationUnit.Step;

    using Result;
    using ThreadingTask = System.Threading.Tasks.Task;

    public class Compiler : ICompiler
    {
        private readonly ICompilationUnitDriver unitDriver;
        private readonly ICompilerConfigurationRepository configurationRepository;
        private readonly IBuildEngine buildEngine;

        public Compiler(ICompilerConfigurationRepository configurationRepository, ICompilationUnitDriver unitDriver, IBuildEngine buildEngine)
        {
            this.configurationRepository = configurationRepository;
            this.unitDriver = unitDriver;
            this.buildEngine = buildEngine;
        }
        
        public ICompilationResult Compile(IPredefinedBuildArgs buildArgs)
        {
#if !DEBUG
            if (buildArgs.AttachDebugger && !Debugger.IsAttached)
            {
                Debugger.Break();
            }
#endif

            return ExecuteStepsByDependency(configurationRepository.GetAllStepConfigurations());
        }

        private CompilationResult ExecuteStepsByDependency(ICompilationStepConfiguration[] allCompilationSteps)
        {
            List<ICompilationUnitResult> results = new List<ICompilationUnitResult>();

            List<ICompilationUnitResult> failedResults = null;

            Dictionary<string, bool> executedSteps = new Dictionary<string, bool>();

            bool failed = false;

            while (executedSteps.Count < allCompilationSteps.Length)
            {
                ICompilationStepConfiguration[] readySteps = GetReadySteps(executedSteps, allCompilationSteps);

                List<Task<ICompilationUnitResult>> readyStepsTasks = new List<Task<ICompilationUnitResult>>();

                foreach (ICompilationStepConfiguration stepConfiguration in readySteps)
                {
                    executedSteps[stepConfiguration.Name] = true;

                    ICompilationUnitArgs unitArgs = buildEngine.MakeExecutionUnitArgs(stepConfiguration);

                    readyStepsTasks.Add(ThreadingTask.Factory.StartNew(() => unitDriver.ExecuteRecursive(unitArgs, stepConfiguration)));
                }

                List<ICompilationUnitResult> readyStepsResults = ThreadingTask.WhenAll(readyStepsTasks).Result.ToList();

                results.AddRange(readyStepsResults);

                failedResults = readyStepsResults.Where(res => !res.BuildSucceeded).ToList();

                if (failedResults.Any())
                {
                    failed = true;
                    break;
                }
            }

            return new CompilationResult
            {
                FailedUnitResults = failedResults?.ToArray() ?? new ICompilationUnitResult[0],
                CompilationSucceeded = !failed,
                UnitResults = results.ToArray()
            };
        }

        private ICompilationStepConfiguration[] GetReadySteps(Dictionary<string, bool> executedSteps, ICompilationStepConfiguration[] allCompilationSteps)
        {
            return allCompilationSteps
                    .Where(step => IsStepReady(executedSteps, step))
                    .ToArray();
        }

        private bool IsStepReady(Dictionary<string, bool> executedSteps, ICompilationStepConfiguration stepConfiguration)
        {
            if (stepConfiguration.DependsOnSteps == null || !stepConfiguration.DependsOnSteps.Any())
            {
                return true;
            }

            return stepConfiguration.DependsOnSteps.All(name => executedSteps.ContainsKey(name));
        }
    }
}