namespace CVB.NET.Rewriting.Compiler.CompilationUnits.Tasks.Cecil.Base
{
    using System;
    using System.Reflection;
    using Mono.Cecil;

    public interface ITypeTransformationAttribute
    {
        void ApplyTransformation(
            Assembly reflectionAssembly,
            AssemblyDefinition cecilAssembly,
            Type reflectionType,
            TypeDefinition cecilType);
    }
}