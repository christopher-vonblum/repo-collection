using System.Diagnostics;

using CVB.NET.Rewriting.Compiler.Helpers;
using CVB.NET.Rewriting.Compiler.Services.Interfaces.Cecil;

namespace CVB.NET.Rewriting.Compiler.CompilationUnits.Tasks.Cecil
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Attributes;

    using Base;

    using CompilationUnit.Argument;
    using CompilationUnit.Result;
    using CompilationUnit.Task;

    using Exception;

    using Mono.Cecil;

    using Utils;

    public class CecilTransformationRunner : CompilationTaskBase<ICecilScopedTransformationConfiguration>
    {
        private readonly IAssemblyDependencyResolver assemblyHelper;

        private readonly ICecilAssemblyTransformationContext transformationContext;

        public CecilTransformationRunner(IAssemblyDependencyResolver assemblyHelper, ICecilAssemblyTransformationContext transformationContext)
        {
            this.assemblyHelper = assemblyHelper;
            this.transformationContext = transformationContext;
        }

        public override ICompilationUnitResult Execute(ICompilationUnitArgs args)
        {
            AggregateException e = null;

            Assembly intermediateAssembly = this.assemblyHelper.ResolveAssembly(args.AssemblyName);

            this.TransformAssembly(intermediateAssembly);

            this.transformationContext.WriteTransformations();

            return CreateResult();
        }

        private void TransformAssembly(Assembly preTransformationAssembly)
        {
            IEnumerable<Type> assemblyTransformationTypes = GetTransformationTypes<IAssemblyTransformationAttribute>(preTransformationAssembly);

            IEnumerable<Type> typeTransformationTypes = GetTransformationTypes<ITypeTransformationAttribute>(preTransformationAssembly);

            if (assemblyTransformationTypes.Any() || typeTransformationTypes.Any())
            {
                this.transformationContext.GetTransformationAssembly(preTransformationAssembly.GetName()).EnsureHasCustomAttribute<ContainsTransformationTypes>();
            }

            Assembly[] referencedAssemblies = this.GetReferencedAssemblies(preTransformationAssembly).Where(asm => asm.CustomAttributes.OfType<ContainsTransformationTypes>().Any()).ToArray();
            
            assemblyTransformationTypes = assemblyTransformationTypes.Concat(GetTransformationTypes<IAssemblyTransformationAttribute>(referencedAssemblies));

            typeTransformationTypes = typeTransformationTypes.Concat(GetTransformationTypes<ITypeTransformationAttribute>(referencedAssemblies));

            ApplyAssemblyTransformations(preTransformationAssembly, assemblyTransformationTypes);

            ApplyTypeTransformations(preTransformationAssembly, typeTransformationTypes);
        }

        private IEnumerable<Assembly> GetReferencedAssemblies(Assembly preTransformationAssembly)
        {
            foreach (Assembly asm in GetReferencedReflectionOnlyAssemblies(preTransformationAssembly))
            {
                Assembly referencedAssembly;

                try
                {
                    referencedAssembly = Assembly.Load(asm.FullName);
                }
                catch
                {
                    continue;
                }

                yield return asm;
            }
        }

        private IEnumerable<Assembly> GetReferencedReflectionOnlyAssemblies(Assembly preTransformationAssembly)
        {
            List<Assembly> collected = new List<Assembly>();

            // System and System.Configuration seem to have a circular dependency O.o
            string assemblyName = preTransformationAssembly.GetName().Name;
            if (assemblyName.StartsWith("System.") || assemblyName.Equals("System"))
            {
                return collected;
            }

            foreach (AssemblyName assemblyReference in preTransformationAssembly.GetReferencedAssemblies())
            {
                Assembly referencedAssembly;
                
                try
                {
                    referencedAssembly = Assembly.ReflectionOnlyLoad(assemblyReference.FullName);
                }
                catch
                {
                    continue;
                }

                collected.Add(referencedAssembly);

                collected.AddRange(this.GetReferencedReflectionOnlyAssemblies(referencedAssembly));
            }

            return collected.Distinct();
        }

        /* Assembly Tranformation */

        private void ApplyAssemblyTransformations(Assembly preTransfromReflectionAssembly, IEnumerable<Type> assemblyTransformationTypes)
        {
            List<System.Exception> exceptions = new List<System.Exception>();

            foreach (Type transformationType in assemblyTransformationTypes)
            {
                Assembly[] transformationTargets = GetAssemblyTransformationTargets(preTransfromReflectionAssembly, transformationType);

                foreach (Assembly transformationTarget in transformationTargets)
                {
                    IAssemblyTransformationAttribute[] typeTransformationAttributes = transformationTarget.GetCustomAttributes(transformationType, false).Select(t => (IAssemblyTransformationAttribute)t).ToArray();

                    foreach (IAssemblyTransformationAttribute transformationAttribute in typeTransformationAttributes)
                    {
                        try
                        {
                            AssemblyDefinition def = this.transformationContext.GetTransformationAssembly(preTransfromReflectionAssembly.GetName());

                            transformationAttribute.ApplyTransformation(preTransfromReflectionAssembly, def);
                        }
                        catch (System.Exception ex)
                        {
                            exceptions.Add(new AssemblyTransformationException(preTransfromReflectionAssembly, transformationType, ex));
                        }
                    }
                }
            }

            if (exceptions.Any())
            {
                throw new AggregateException(exceptions);
            }
        }

        private Assembly[] GetAssemblyTransformationTargets(Assembly preTransformReflectionAssembly, Type transformationType)
        {
            if (preTransformReflectionAssembly.HasCustomAttribute(transformationType))
            {
                return new[] { preTransformReflectionAssembly };
            }

            return new Assembly[0];
        }

        /* Type Tranformation */

        private void ApplyTypeTransformations(Assembly preTransfromReflectionAssembly, IEnumerable<Type> typeTransformationTypes)
        {
            List<System.Exception> exceptions = new List<System.Exception>();

            foreach (Type transformationType in typeTransformationTypes)
            {
                Type[] transformationTargets = GetTypeTransformationTargets(preTransfromReflectionAssembly, transformationType);

                foreach (Type transformationTarget in transformationTargets)
                {
                    ITypeTransformationAttribute[] typeTransformationAttributes = transformationTarget.GetCustomAttributes(transformationType, false).Select(t => (ITypeTransformationAttribute)t).ToArray();

                    foreach (ITypeTransformationAttribute transformationAttribute in typeTransformationAttributes)
                    {
                        try
                        {
                            AssemblyDefinition def = this.transformationContext.GetTransformationAssembly(preTransfromReflectionAssembly.GetName());

                            transformationAttribute.ApplyTransformation(preTransfromReflectionAssembly, def, transformationTarget, (TypeDefinition)def.MainModule.LookupToken(transformationTarget.MetadataToken));
                        }
                        catch (System.Exception ex)
                        {
                            exceptions.Add(new TypeTransformationException(transformationTarget, transformationTarget, ex));
                        }
                    }
                }
            }

            if (exceptions.Any())
            {
                throw new AggregateException(exceptions);
            }
        }

        private Type[] GetTypeTransformationTargets(Assembly preTransformReflectionAssembly, Type transformationType)
        {
            Type[] assemblyTypes = preTransformReflectionAssembly.GetTypes().OrderBy(t => t.Name).ToArray();

            return assemblyTypes.Where(t => t.HasCustomAttribute(transformationType)).ToArray();
        }

        /* Generic */

        private IEnumerable<Type> GetTransformationTypes<TTransformationInterface>(params Assembly[] assemblies)
        {
            IEnumerable<Type> transformationTypes = new List<Type>();

            foreach (Assembly assemblyReference in assemblies)
            {
                Type[] assemblyTransformationTypes;

                assemblyTransformationTypes = assemblyReference.GetTypes().Where(t => typeof(TTransformationInterface).IsAssignableFrom(t) && !t.IsInterface).ToArray();

                transformationTypes = transformationTypes.Concat(assemblyTransformationTypes);
            }

            return transformationTypes;
        }
    }
}