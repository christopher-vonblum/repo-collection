namespace CVB.NET.Rewriting.Compiler.Configuration.Models.CompilationUnit
{
    using System;
    using System.Collections.Generic;
    using NET.Configuration.Base;

    public interface ICompilationUnitConfiguration : IConfigurationElement
    {
        /// <summary>
        /// The name of the unit. Can be duplicated for steps and tasks.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Tasks to execute before the units core action.
        /// </summary>
        string[] PreExecutionTasks { get; }

        
        bool ExecutePreExecutionTasksOrdered { get; }

        /// <summary>
        /// Tasks to execute after the units core action.
        /// </summary>
        string[] PostExecutionTasks { get; }

        bool ExecutePostExecutionTasksOrdered { get; }

        /// <summary>
        /// Implementation type of the execution unit.
        /// </summary>
        Type ImplementationType { get; }

        /// <summary>
        /// Controls isolation behavior.
        /// </summary>
        bool IsolateUnit { get; }

        /// <summary>
        /// Custom parameters passed to the execution unit.
        /// </summary>
        IReadOnlyDictionary<string, string> Parameters { get; }

        /// <summary>
        /// Configuration implementation type that depicts the <see cref="Parameters" /> as a strongly typed model. 
        /// </summary>
        Type ConfigurationImplementationType { get; }
    }
}