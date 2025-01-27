using System;
using System.IO;
using Microsoft.Build.Framework;
using Task = Microsoft.Build.Utilities.Task;

namespace MsBuildExtension
{
    public class RewritingTask : Task
    {
        [Required]
        public string OutputAssembly { get; set; }

        [Required]
        public string BaseIntermediateOutputPath { get; set; }

        [Output]
        public string PreTransformationAssembly { get; set; }

        public override bool Execute()
        {
            string preTransformFolder = BaseIntermediateOutputPath + "PreTransform\\";

            Log(nameof(preTransformFolder) + "=" + preTransformFolder + ";");

            string baseIntermediateAssembly = BaseIntermediateOutputPath + Path.GetFileName(OutputAssembly);

            Log(nameof(baseIntermediateAssembly) + "=" + baseIntermediateAssembly + ";");

            string preTransformAssembly = preTransformFolder + Path.GetFileName(OutputAssembly);

            Log(nameof(preTransformAssembly) + "=" + preTransformAssembly + ";");

            if (!Directory.Exists(preTransformFolder))
            {
                Log($"Creating {nameof(preTransformFolder)}({preTransformFolder})");

                Directory.CreateDirectory(preTransformFolder);
            }

            if (!File.Exists(baseIntermediateAssembly))
            {
                Log("Intermediate assembly is missing. Could not proceed with the transformation.");

                return false;
            }

            File.Copy(baseIntermediateAssembly, preTransformAssembly, true);

            Log($"Copied baseIntermediateAssembly({baseIntermediateAssembly}) to {preTransformAssembly}");

            return true;
        }

        private void Log(string message)
        {
            if (!File.Exists("C:\\log.txt"))
            {
                File.WriteAllText("C:\\log.txt", message + "\r\n");

                return;
            }

            File.AppendAllText("C:\\log.txt", message + "\r\n");
        }
    }
}
