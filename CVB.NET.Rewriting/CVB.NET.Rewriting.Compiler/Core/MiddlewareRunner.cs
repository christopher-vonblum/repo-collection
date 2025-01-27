using System.Reflection;
using System.Runtime.Loader;
using CVB.NET.Rewriting.CompilerArgs;

namespace CVB.NET.Rewriting.Core;

public class MiddlewareRunner
{
    public Result RunMiddlewarePlugins(IArgs arguments)
    {
        foreach (string rewritingMiddlewareType in arguments.RewritingMiddlewareTypes)
        {
            // Split qualified assembly name.
            string[] splittedQualifiedName = rewritingMiddlewareType.Split(',');

            // Extract middleware-assemblies name.
            string rewritingMiddlewareAssembly = splittedQualifiedName[1].TrimEnd();

            // load middleware assembly
            MiddlewarePluginAssemblyLoadContext loadContext =
                new MiddlewarePluginAssemblyLoadContext(arguments.CompilerInstallationPath, );
        }
        
        
    }
}


