using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using CVB.NET.Rewriting.Compiler.BuildIntegration;
using Unity.Attributes;

namespace CVB.NET.Rewriting.Compiler.Helpers
{
    public class AssemblyDependencyResolver : IAssemblyDependencyResolver
    {
        private Dictionary<string, string> assemblyPathLookup = new Dictionary<string, string>();

        public AssemblyDependencyResolver(IEnumerable<string> searchPaths)
        {
            searchPaths
                .SelectMany(path => Directory.GetFiles(path))
                .Where(file => Path.GetExtension(file).ToLower() == ".exe" || Path.GetExtension(file).ToLower() == ".dll")
                .ToList()
                .ForEach(a =>
                {
                    AssemblyName assemblyName;

                    if (!TryGetAssemblyName(a, out assemblyName))
                    {
                        return;
                    }

                    assemblyPathLookup.Add(assemblyName.FullName, a);
                });

            AppDomain.CurrentDomain.AssemblyResolve
                += (sender, eventArgs) =>
                       this.ResolveAssemblyInternal(new AssemblyName(eventArgs.Name));
        }

        [InjectionConstructor]
        public AssemblyDependencyResolver(IBuildEngine engine) : this(engine.GetFileResolverSearchPaths())
        {
        }

        private Assembly ResolveAssemblyInternal(AssemblyName assemblyName)
        {
            return this.assemblyPathLookup.ContainsKey(assemblyName.FullName) ? Assembly.LoadFile(this.assemblyPathLookup[assemblyName.FullName]) : null;
        }

        public Assembly ResolveAssembly(AssemblyName assemblyName)
        {
            return ResolveAssemblyInternal(assemblyName) ?? Assembly.Load(assemblyName);
        }

        private bool TryGetAssemblyName(string assemblyPath, out AssemblyName assemblyName)
        {
            try
            {
                assemblyName = Assembly.ReflectionOnlyLoadFrom(assemblyPath).GetName();
                return true;
            }
            catch
            {
                assemblyName = null;
                return false;
            }
        }
        
        public string ResolveAssemblySourcePath(AssemblyName assemblyName)
        {
            return this.assemblyPathLookup.ContainsKey(assemblyName.FullName) ? this.assemblyPathLookup[assemblyName.FullName] : null;
        }
    }
}