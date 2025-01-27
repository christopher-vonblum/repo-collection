using CVB.NET.Rewriting.Compiler.Helpers;

namespace CVB.NET.Rewriting.Compiler.CompilationUnits.Tasks.PostSharp
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.IO.Compression;
    using System.Net;
    using CompilationUnit.Argument;
    using CompilationUnit.Result;
    using CompilationUnit.Task;
    using Error;

    public class PostSharpTask : CompilationTaskBase<IPostsharpCompilationConfiguration>
    {
        private readonly IIntermediateFileHelper fileHelper;

        public PostSharpTask(IIntermediateFileHelper fileHelper)
        {
            this.fileHelper = fileHelper;
        }

        public override ICompilationUnitResult Execute(ICompilationUnitArgs args)
        {
            Uri downloadUri = new Uri(Configuration.BinaryDownloadUri);

            string downloadPath = args.PreTransformationPath + "___postsharp_assemblies\\";

            string localPath = downloadPath + Path.GetFileName(downloadUri.AbsolutePath);

            if (!Directory.Exists(downloadPath))
            {
                Directory.CreateDirectory(downloadPath);
            }

            if (!File.Exists(localPath))
            {
                WebClient client = new WebClient();
                client.DownloadFile(Configuration.BinaryDownloadUri, localPath);
            }

            string localDirectory = downloadPath + Path.GetFileNameWithoutExtension(downloadUri.AbsolutePath);

            if (!Directory.Exists(localDirectory))
            {
                ZipFile.ExtractToDirectory(localPath, localDirectory);
            }

            string buildTemplate = $@"<Project xmlns=""http://schemas.postsharp.org/1.0/configuration"">
                                        <Property Name=""Input"" Value=""{fileHelper.GetPreTransformationFilePath(args.TransformationOutputAssembly)}"" />
                                        <Property Name=""Output"" Value=""{args.AssemblyName}"" />
                                        <Property Name=""LoggingBackend"" Value=""Console"" />
                                    </Project>";

            string postsharpScriptPath = args.TransformationOutputPath + "\\postsharp.config";
            
            if (File.Exists(postsharpScriptPath))
            {
                File.Delete(postsharpScriptPath);
            }

            File.WriteAllText(postsharpScriptPath, buildTemplate);

            Process postSharp = new Process();

            postSharp.StartInfo.CreateNoWindow = true;
            postSharp.StartInfo.FileName = localPath + "\\tools\\postsharp-net40-x86-native.exe";
            postSharp.StartInfo.Arguments = postsharpScriptPath;

            postSharp.Start();

            postSharp.WaitForExit();

            return new CompilationUnitResult
            {
                BuildSucceeded = true,
                CompilationErrors = new ICompilationError[0],
            };
        }
    }
}
