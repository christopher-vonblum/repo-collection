using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CVB.NET.Rewriting.Compiler.Argument;
using CVB.NET.Rewriting.Compiler.CompilationUnit.Argument;
using CVB.NET.Rewriting.Compiler.CompilationUnit.Result;
using CVB.NET.Rewriting.Compiler.Configuration;
using CVB.NET.Rewriting.Compiler.Configuration.Models;
using CVB.NET.Rewriting.Compiler.Configuration.Models.CompilationUnit;
using CVB.NET.Rewriting.Compiler.Error;
using Unity;
using Unity.Attributes;

namespace CVB.NET.Rewriting.Compiler.CompilationUnit.Driver
{
    public class CompilationUnitDriver : MarshalByRefObject, ICompilationUnitDriver
    {
        private ICompilationUnitRunner localRunner;
        private IUnityContainer unityContainer;
        private IPredefinedBuildArgs predefinedBuildArgs;
        private ICompilerConfigurationRepository executionUnitConfigurationRepository;
        private ICompilerConfiguration configuration;

        [InjectionMethod]
        public void Inject(ICompilationUnitRunner localRunner, IPredefinedBuildArgs predefinedBuildArgs, ICompilerConfigurationRepository executionUnitConfigurationRepository, ICompilerConfiguration configuration)
        {
            this.localRunner = localRunner;
            this.predefinedBuildArgs = predefinedBuildArgs;
            this.executionUnitConfigurationRepository = executionUnitConfigurationRepository;
            this.configuration = configuration;
        }

        /// <summary>
        /// Executes the step or task with its pre and post actions.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="unitConfiguration"></param>
        /// <returns></returns>
        public ICompilationUnitResult ExecuteRecursive(ICompilationUnitArgs args, ICompilationUnitConfiguration unitConfiguration)
        {
            if (unitConfiguration.PreExecutionTasks != null)
            {
                ICompilationUnitResult preResult = ExecuteTasksByName(args, unitConfiguration, unitConfiguration.PreExecutionTasks, !unitConfiguration.ExecutePreExecutionTasksOrdered);

                if (!preResult.BuildSucceeded)
                {
                    return preResult;
                }
            }

            ICompilationUnitResult coreResult;

            if (unitConfiguration.IsolateUnit)
            {
                coreResult = this.CreateIsolatedEvaluationTask(args, unitConfiguration).Result;
            }
            else
            {
                coreResult = this.CreateEvaluationTask(args, unitConfiguration).Result;
            }
            
            if (!coreResult.BuildSucceeded)
            {
                return coreResult;
            }

            if (unitConfiguration.PostExecutionTasks != null)
            {
                ICompilationUnitResult postResult = ExecuteTasksByName(args, unitConfiguration, unitConfiguration.PostExecutionTasks, !unitConfiguration.ExecutePostExecutionTasksOrdered);

                if (!postResult.BuildSucceeded)
                {
                    return postResult;
                }
            }
            
            return coreResult;
        }

        private ICompilationUnitResult ExecuteTasksByName(ICompilationUnitArgs args, ICompilationUnitConfiguration parentUnitConfiguration, string[] taskNames, bool multiThreading)
        {
            ICompilationUnitConfiguration[] configurations = taskNames
                                                                    .Select(configName => executionUnitConfigurationRepository.GetTaskConfiguration(configName))
                                                                    .ToArray();

            ICompilationUnitResult[] taskResults = multiThreading ? this.ExecuteTasksMultiThreaded(args, configurations) : this.ExecuteTasksOrdered(args, configurations);

            return new CompilationUnitResult
            {
                BuildSucceeded = taskResults.All(res => res.BuildSucceeded),
                UnitConfiguration = parentUnitConfiguration,
                CompilationErrors = new ICompilationError[0],
                ChildResults = taskResults
            };
        }

        private ICompilationUnitResult[] ExecuteTasksOrdered(ICompilationUnitArgs args, IEnumerable<ICompilationUnitConfiguration> taskConfigurations)
        {
            List<ICompilationUnitResult> results = new List<ICompilationUnitResult>();

            foreach (var compilationUnitConfiguration in taskConfigurations)
            {
                results.Add(ExecuteRecursive(args, compilationUnitConfiguration));
            }

            return results.ToArray();
        }

        private ICompilationUnitResult[] ExecuteTasksMultiThreaded(ICompilationUnitArgs args, IEnumerable<ICompilationUnitConfiguration> taskConfigurations)
        {
            IEnumerable<Task<ICompilationUnitResult>> tasks = taskConfigurations.Select(t => Task<ICompilationUnitResult>.Factory.StartNew(() => ExecuteRecursive(args, t)));

            return System.Threading.Tasks.Task.WhenAll(tasks).Result;
        }
        
        private Task<ICompilationUnitResult> CreateIsolatedEvaluationTask(ICompilationUnitArgs args, ICompilationUnitConfiguration unitConfiguration)
        {
            return Task<ICompilationUnitResult>.Factory.StartNew(() =>
            {
                AppDomain isolationDomain = AppDomain.CreateDomain(Guid.NewGuid().ToString("B"), AppDomain.CurrentDomain.Evidence, AppDomain.CurrentDomain.SetupInformation);

                Type runnerType = typeof(CompilationUnitRunner);

                CompilationUnitRunner remoteRunner = (CompilationUnitRunner)isolationDomain.CreateInstanceAndUnwrap(runnerType.Assembly.FullName, runnerType.FullName);

                remoteRunner.BootstrapRunnerDomain(predefinedBuildArgs, this.configuration);

                ICompilationUnitResult result = remoteRunner.EvaluateRemote(args, unitConfiguration.Name);

                this.WireEvaluationResult(result, unitConfiguration);

                AppDomain.Unload(isolationDomain);
                
                return result;
            });
        }

        /// <summary>
        /// TODO: Ugly hack because unit configurations are not serializable. (remoting to runner domain...)
        /// </summary>
        /// <param name="result"></param>
        /// <param name="configuration"></param>
        private void WireEvaluationResult(ICompilationUnitResult result, ICompilationUnitConfiguration configuration)
        {
            ((CompilationUnitResult)result).UnitConfiguration = configuration;
        }

        private Task<ICompilationUnitResult> CreateEvaluationTask(ICompilationUnitArgs args, ICompilationUnitConfiguration unitConfiguration)
        {
            return Task<ICompilationUnitResult>.Factory.StartNew(
                () =>
                    {
                        var result = localRunner.Evaluate(args, unitConfiguration);
                        this.WireEvaluationResult(result, unitConfiguration);
                        return result;
                    });
        }
    }
}