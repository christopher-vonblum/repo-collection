using System;
using System.Collections.Generic;
using System.Reflection;
using CVB.NET.Rewriting.Compiler.BuildIntegration;
using CVB.NET.Rewriting.Compiler.Helpers;
using CVB.NET.Rewriting.Compiler.Services.Cecil.Integration;
using CVB.NET.Rewriting.Compiler.Services.Interfaces.Cecil;

using Mono.Cecil;

namespace CVB.NET.Rewriting.Compiler.Services.Cecil
{
    public class CecilAssemblyTransformationContext : ICecilAssemblyTransformationContext, IDisposable
    {
        private readonly IBuildEngine buildEngine;
        private readonly IIntermediateFileHelper intermediateFileHelper;
        private readonly IAssemblyDependencyResolver reflectionAssemblyHelper;

        private readonly Dictionary<string, (string assemblyFile, AssemblyDefinition transformationAssembly)> assemblyTransformations = new Dictionary<string, (string assemblyFile, AssemblyDefinition transformationAssembly)>();

        public CecilAssemblyTransformationContext(IBuildEngine buildEngine, IIntermediateFileHelper intermediateFileHelper, IAssemblyDependencyResolver reflectionAssemblyHelper)
        {
            this.buildEngine = buildEngine;
            this.intermediateFileHelper = intermediateFileHelper;
            this.reflectionAssemblyHelper = reflectionAssemblyHelper;
        }

        public AssemblyDefinition GetTransformationAssembly(AssemblyName reflectionAssembly)
        {
            string preTransformationFilePath = reflectionAssemblyHelper.ResolveAssemblySourcePath(reflectionAssembly);

            return this.GetOrAddTransformationAssembly(reflectionAssembly.FullName, preTransformationFilePath);
        }
        
        private AssemblyDefinition GetOrAddTransformationAssembly(string assemblyName, string assemblyFile)
        {
            return (this.assemblyTransformations.ContainsKey(assemblyName) 
                      ? this.assemblyTransformations[assemblyName]
                      : (this.assemblyTransformations[assemblyName] = (assemblyFile, 
                                                                       AssemblyDefinition.ReadAssembly(assemblyFile, 
                                                                           new ReaderParameters
                                                                           {
                                                                               ReadSymbols = true,
                                                                               AssemblyResolver = new BuildEngineDrivenAssemblyResolver(buildEngine)
                                                                           }))))
                   .transformationAssembly;
        }

        public void WriteTransformations()
        {
            foreach ((string assemblyFile, AssemblyDefinition transformationAssembly) in this.assemblyTransformations.Values)
            {
                transformationAssembly.Write(this.intermediateFileHelper.GetTransformationFilePath(assemblyFile), new WriterParameters { WriteSymbols = true });
            }

            assemblyTransformations.Clear();
        }

        public void Dispose()
        {
            WriteTransformations();
        }
    }
}