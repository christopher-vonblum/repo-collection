using System.Reflection;
using System.Runtime.Loader;

namespace CVB.NET.Rewriting.Core;

public class MiddlewarePluginAssemblyLoadContext : AssemblyLoadContext
{
    private readonly string _installationLocation;
    private readonly string[] _middlewareLoadPaths;

    public MiddlewarePluginAssemblyLoadContext(string installationLocation, string[] middlewareLoadPaths)
    {
        _installationLocation = installationLocation;
        _middlewareLoadPaths = middlewareLoadPaths;
    }
    
    protected override Assembly? Load(AssemblyName assemblyName)
    {
        foreach (string middlewareLoadPath in _middlewareLoadPaths)
        {
            if (File.Exists(Path.Combine(middlewareLoadPath, assemblyName.FullName + ".dll")))
            {
                return LoadFromAssemblyPath(middlewareLoadPath);
            }
        }
        
        if (File.Exists(_installationLocation))
        {
            return LoadFromAssemblyPath(Path.Combine(_installationLocation, assemblyName.FullName + ".dll"));
        }

        return null;
    }
}