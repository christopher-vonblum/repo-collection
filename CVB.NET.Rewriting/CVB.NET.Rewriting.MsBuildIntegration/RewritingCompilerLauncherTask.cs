using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using CVB.NET.Rewriting.CompilerArgs;
using Microsoft.Build.Framework;
using Newtonsoft.Json;

namespace CVB.NET.Rewriting.MsBuildIntegration
{
    public class RewritingCompilerLauncherTask : ITask, IArgs
    {
        /// <inheritdoc />
        public IBuildEngine BuildEngine { get; set; }
        
        /// <inheritdoc />
        
        public ITaskHost HostObject { get; set; }

        /// <inheritdoc />
        public string CompilerInstallationPath { get; set; }
        
        /// <inheritdoc />
        
        public string CompilerResultDocumentPath { get; set; }
        
        /// <inheritdoc />
        public string IntermediateOutputPath { get; set; }
        
        /// <inheritdoc />
        public string IntermediateAssemblyPath { get; set; }

        /// <inheritdoc />
        public List<string> RewritingMiddlewareTypes { get; set; }

        /// <inheritdoc />
        public bool Execute()
        {
            // Process used to launch the rewriting-compiler.
            Process compilationProcess = new Process();

            // Path where we create the arguments file.
            string compilerArgsFile = $"{IntermediateOutputPath}{Path.DirectorySeparatorChar}args.json";
            
            // Path where the rewriting-compiler-process dumps the output-document.
            string compilerOutputFile =
                $"{IntermediateOutputPath}{Path.DirectorySeparatorChar}result.json";

            // Set the path to the compiler-executable.
            compilationProcess.StartInfo.FileName =
                $"{CompilerInstallationPath}{Path.DirectorySeparatorChar}CVB.NET.Rewriting.Compiler.exe";
            
            // pass the args.json location to the compiler-executable.
            compilationProcess.StartInfo.Arguments = compilerArgsFile;

            // serialize this to args.json that contains the compiler arguments and write them to file.
            File.WriteAllText(
                compilerArgsFile,
                JsonConvert.SerializeObject(this, typeof(IArgs),
                    new JsonSerializerSettings() { Formatting = Formatting.Indented }));

            // start the standalone-executable CVB.NET.Rewriting.Compiler.
            compilationProcess.Start();
            
            // await the executables processes ending so that we can grab the result.json.
            compilationProcess.WaitForExit();

            // read the result file that the standalone-executable dumped.
            Result result = JsonConvert.DeserializeObject<Result>(File.ReadAllText(compilerOutputFile));
            
            // report errors from the result file to msbuild.
            foreach (string error in result.Errors)
            {
                BuildEngine.LogErrorEvent(
                    new BuildErrorEventArgs(
                        "",
                        "",
                        "",
                        0,
                        0,
                        0,
                        0,
                        error,
                        "",
                        ""));
            }

            // report warnings from the result file to msbuild.
            foreach (string warning in result.Warnings)
            {
                BuildEngine.LogWarningEvent(
                    new BuildWarningEventArgs(
                        "",
                        "",
                        "",
                        0,
                        0,
                        0,
                        0,
                        warning,
                        "",
                        ""));
            }
            
            // report message from the result file to msbuild.
            foreach (string message in result.Messages)
            {
                BuildEngine.LogWarningEvent(
                    new BuildWarningEventArgs(
                        "",
                        "",
                        "",
                        0,
                        0,
                        0,
                        0,
                        message,
                        "",
                        ""));
            }

            // check for errors and mark the build as failed if there are errors.
            return !result.Errors.Any();
        }
    }
}
