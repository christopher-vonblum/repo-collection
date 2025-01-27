using System.Diagnostics;
using Microsoft.Build.Framework;

namespace RadFramework.Features.MsBuildExtensionsAdapter
{
    public class GenericBuildExtensionAdapterTask : ITask
    {
        public IBuildEngine BuildEngine { get; set; }
        public ITaskHost HostObject { get; set; }

        public string ParametersJson { get; set; }
        public string Executable { get; set; }

        public bool Execute()
        {
            
            return true;
        }
    }
}