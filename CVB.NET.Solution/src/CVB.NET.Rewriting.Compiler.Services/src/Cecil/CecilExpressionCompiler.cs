using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using CVB.NET.Rewriting.Compiler.BuildIntegration;
using CVB.NET.Rewriting.Compiler.Services.Cecil.Integration;
using CVB.NET.Rewriting.Compiler.Services.Interfaces.Cecil;
using Mono.Cecil;

namespace CVB.NET.Rewriting.Compiler.Services.Cecil
{
    public class CecilExpressionCompiler : ICecilExpressionCompiler
    {
        private readonly IBuildEngine buildEngine;

        public CecilExpressionCompiler(IBuildEngine buildEngine)
        {
            this.buildEngine = buildEngine;
        }

        public MethodDefinition CompileToMethodDefinition<TDelegate>(BlockExpression block, params ParameterExpression[] parameters)
        {
            string assemblyFileName;
            int metadataToken;

            CompileToTemporaryAssembly<TDelegate>(block, parameters, out assemblyFileName, out metadataToken);

            return (MethodDefinition)AssemblyDefinition.ReadAssembly(assemblyFileName, new ReaderParameters
                                                                                       {
                                                                                           AssemblyResolver = new BuildEngineDrivenAssemblyResolver(buildEngine)
                                                                                       }).MainModule.LookupToken(metadataToken);
        }

        private void CompileToTemporaryAssembly<TDelegate>(BlockExpression block, ParameterExpression[] parameters, out string assemblyFileName, out int methodMetadataToken)
        {
            string assemblyName = $"CecilExpressionAdapterIntermediateAssembly.{Guid.NewGuid():N}";

            assemblyFileName = assemblyName + ".dll";

            AssemblyBuilder assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(new AssemblyName(assemblyName), AssemblyBuilderAccess.Save);
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName, assemblyFileName);
            TypeBuilder typeBuilder = moduleBuilder.DefineType("LambdaContainer", System.Reflection.TypeAttributes.Public | System.Reflection.TypeAttributes.Class);

            Type tDelegate = typeof(TDelegate);

            Type returnType = GetReturnType(tDelegate);

            Type[] parameterTypes = GetParameterTypes(tDelegate);

            MethodBuilder methodBuilder = typeBuilder.DefineMethod("MyMethod", System.Reflection.MethodAttributes.HideBySig | System.Reflection.MethodAttributes.Static | System.Reflection.MethodAttributes.Public, returnType, parameterTypes);

            Expression<TDelegate> lambda = Expression.Lambda<TDelegate>(block, parameters);

            lambda.CompileToMethod(methodBuilder);

            typeBuilder.CreateType();

            assemblyBuilder.Save(assemblyFileName);
            
            methodMetadataToken = methodBuilder.GetToken().Token;
        }

        private Type[] GetParameterTypes(Type delegateType)
        {
            if (!delegateType.Name.StartsWith("Func"))
            {
                return delegateType.GenericTypeArguments;
            }

            List<Type> parameterTypes = delegateType.GenericTypeArguments.ToList();
            parameterTypes = parameterTypes.GetRange(0, parameterTypes.Count - 1);

            return parameterTypes.ToArray();
        }

        private Type GetReturnType(Type delegateType)
        {
            return delegateType.Name.StartsWith("Func") ? delegateType.GetGenericArguments().Last() : typeof(void);
        }
    }
}