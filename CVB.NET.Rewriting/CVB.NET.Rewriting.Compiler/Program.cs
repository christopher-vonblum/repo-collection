using CVB.NET.Rewriting.CompilerArgs;
using Newtonsoft.Json;

namespace CVB.NET.Rewriting.Compiler
{
    /// <summary>
    /// Contains the entry point for the rewriting-compiler.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Entry point for the rewriting-compiler.
        /// </summary>
        /// <param name="cmdArgs">command line args.</param>
        public static void Main(string[] cmdArgs)
        {
            // the input Args file as json document.
            string inputJsonPath = cmdArgs[0];

            // The argument strings that the rewriting compiler needs in order to apply IL modifications get loaded from file.
            // The input file gets provided by an MsBuild ITask that gathers the information that afterwards runs this program. 
            Args arguments = JsonConvert
                .DeserializeObject<Args>(
                    File.ReadAllText(inputJsonPath));
            
            // Execute the middleware classes that apply IL rewriting.
            Result result = new MiddlewareRunner()
                .RunMiddlewarePlugins(arguments);
            
            // Output errors, warnings and messages to the output file.
            // This file will be read again by the MsBuildIntegration-dll and passed to the IDE´s build-output.
            File.WriteAllText(
                arguments.CompilerResultDocumentPath, 
                JsonConvert.SerializeObject(
                    result));
        }
    }
}