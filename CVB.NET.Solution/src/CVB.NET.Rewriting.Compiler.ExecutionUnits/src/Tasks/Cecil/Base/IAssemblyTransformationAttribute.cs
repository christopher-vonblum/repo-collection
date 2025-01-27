namespace CVB.NET.Rewriting.Compiler.CompilationUnits.Tasks.Cecil.Base
{
    using System.Reflection;
    using Mono.Cecil;

    public interface IAssemblyTransformationAttribute
    {
        void ApplyTransformation(Assembly preTransformationReflectionAssembly, AssemblyDefinition postTransformationCecilAssembly);
    }
}