using System.Reflection;
using System.Runtime.Loader;
using CVB.NET.Rewriting.CompilerArgs;

namespace CVB.NET.Rewriting;

public class MiddlewareRunner
{
    public Result RunMiddlewarePlugins(IArgs arguments)
    {
        foreach (string rewritingMiddlewareType in arguments.RewritingMiddlewareTypes)
        {
            // Split qualified assembly name.
            string[] splittedQualifiedName = rewritingMiddlewareType.Split(',');

            // Extract middleware-assemblies name.
            string rewritingMiddlewareAssembly = splittedQualifiedName[1].TrimStart();

            // load middleware assembly
        }
    }
}

public interface IRewritingMiddleware
    {
        void ApplyIlTransformations(IArgs args);
    }

    private abstract class RewritingMiddlewareBase : IRewritingMiddleware
    {
        public abstract void ApplyRewriting(IArgs args, ref Assembly);
        public void ApplyIlTransformations(IArgs args)
        {
            throw new NotImplementedException();
        }
    }

    public CustomCompilerMiddlewareAssemblyLoadContext BuildMiddlewareAssemblyPluginIsolation(string assemblyName)
    {
        
    }
    
    public class CustomCompilerMiddlewareAssemblyLoadContext : AssemblyLoadContext
    {
        private IArgs arguments;
        
        public CustomCompilerMiddlewareAssemblyLoadContext(IArgs arguments)
        {
            this.arguments = arguments;
        }
        protected override Assembly? Load(AssemblyName assemblyName)
        {
            
            
        }
    }
}

