using CVB.NET.Rewriting.Compiler.Argument;
using CVB.NET.Rewriting.Compiler.Services.Interfaces.MsBuild;
using Mono.Cecil;

namespace CVB.NET.Rewriting.Compiler.CompilationUnits.Tasks.PeVerify
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using CompilationUnit.Argument;
    using CompilationUnit.Result;
    using CompilationUnit.Task;
    using Error;
    using Errors;
    using Services.Interfaces.Cecil;
    using Services.Interfaces.Roslyn;

    public class PeVerifyTask : CompilationTaskBase
    {
        private static string chosenProbingPath;

        private readonly IRoslynTransformationContext roslynTransformationContext;

        private readonly AssemblyDefinition assembly;
        private readonly IMsBuildProject project;

        private static readonly string[] PeVerifyProbingPaths =
        {
            @"C:\Program Files\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6.1 Tools",
            @"C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6.1 Tools",
            @"C:\Program Files\Microsoft SDKs\Windows\v8.1A\bin\NETFX 4.5.1 Tools",
            @"C:\Program Files (x86)\Microsoft SDKs\Windows\v8.1A\bin\NETFX 4.5.1 Tools",
            @"C:\Program Files\Microsoft SDKs\Windows\v8.0A\bin\NETFX 4.0 Tools",
            @"C:\Program Files (x86)\Microsoft SDKs\Windows\v8.0A\bin\NETFX 4.0 Tools",
            @"C:\Program Files\Microsoft SDKs\Windows\v7.1\Bin\NETFX 4.0 Tools",
            @"C:\Program Files (x86)\Microsoft SDKs\Windows\v7.1\Bin\NETFX 4.0 Tools",
            @"C:\Program Files\Microsoft SDKs\Windows\v7.0A\bin\NETFX 4.0 Tools",
            @"C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\Bin\NETFX 4.0 Tools",
            @"C:\Program Files\Microsoft SDKs\Windows\v7.0A\Bin",
            @"C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\Bin",
            @"C:\Program Files\Microsoft SDKs\Windows\v6.0A\Bin",
            @"C:\Program Files (x86)\Microsoft SDKs\Windows\v6.0A\Bin",
            @"C:\Program Files (x86)\Microsoft Visual Studio 8\SDK\v2.0\bin"
        };

        public PeVerifyTask(IRoslynTransformationContext roslynTransformationContext, AssemblyDefinition assembly, IMsBuildProject project)
        {
            this.roslynTransformationContext = roslynTransformationContext;
            this.assembly = assembly;
            this.project = project;
        }

        private static string FindPeVerifyPath()
        {
            foreach (string path in PeVerifyProbingPaths)
            {
                string file = Path.Combine(path, "peverify.exe");

                if (File.Exists(file))
                {
                    return file;
                }
            }

            throw new FileNotFoundException("Please check the PeVerifyProbingPaths configuration setting and set it to the folder where peverify.exe is located");
        }

        public static bool Verify(string assemblyPath, out string[] errors)
        {
            Process process = new Process
                          {
                              StartInfo =
                              {
                                  FileName = chosenProbingPath ?? (chosenProbingPath = FindPeVerifyPath()),
                                  RedirectStandardOutput = true,
                                  UseShellExecute = false,
                                  WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory,
                                  Arguments = "\"" + assemblyPath + "\" /VERBOSE",
                                  CreateNoWindow = true
                              }
                          };

            process.Start();

            List<string> msgs = new List<string>();

            while (!process.StandardOutput.EndOfStream)
            {
                msgs.Add(process.StandardOutput.ReadLine());
            }

            process.WaitForExit();

            errors = msgs.Where(msg => msg.StartsWith("[")).ToArray();

            if (process.ExitCode != 0)
            {
                return false;
            }

            return true;
        }

        public override ICompilationUnitResult Execute(ICompilationUnitArgs args)
        {
            string[] peVerifyErrors;

            return new CompilationUnitResult
                   {
                       BuildSucceeded = Verify(project.IntermediateAssembly, out peVerifyErrors),
                       UnitConfiguration = Configuration,
                       CompilationErrors = peVerifyErrors.Select(err => new VerificationError(this, err, roslynTransformationContext, assembly)).ToArray<ICompilationError>()
                   };
        }
    }
}