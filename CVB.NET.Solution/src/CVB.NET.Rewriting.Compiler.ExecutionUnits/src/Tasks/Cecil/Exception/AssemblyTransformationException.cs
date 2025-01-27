namespace CVB.NET.Rewriting.Compiler.CompilationUnits.Tasks.Cecil.Exception
{
    using System;
    using System.Reflection;
    using Error;

    [Serializable]
    public class AssemblyTransformationException : CompilationException
    {
        public AssemblyTransformationException(Assembly targetAssembly, Type assemblyTransformationType, Exception innerException) : base(GetMessage(targetAssembly, assemblyTransformationType), innerException)
        {
            AddCodeLocation(assemblyTransformationType.Assembly, assemblyTransformationType);
        }

        private static string GetMessage(Assembly targetAssembly, Type assemblyTransformationType)
        {
            return $"Assembly transformation failed for custom transformation \"{assemblyTransformationType.FullName}\" on target assembly \"{targetAssembly.FullName}\".";
        }
    }
}