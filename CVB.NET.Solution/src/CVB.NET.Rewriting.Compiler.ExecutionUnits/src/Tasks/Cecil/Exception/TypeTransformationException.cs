namespace CVB.NET.Rewriting.Compiler.CompilationUnits.Tasks.Cecil.Exception
{
    using System;
    using Error;

    [Serializable]
    public class TypeTransformationException : CompilationException
    {
        public TypeTransformationException(Type targetType, Type typeTransformationType, Exception innerException) : base(GetMessage(targetType, typeTransformationType), innerException)
        {
            AddCodeLocation(targetType.Assembly, targetType);
            AddCodeLocation(typeTransformationType.Assembly, typeTransformationType);
        }

        private static string GetMessage(Type targetType, Type typeTransformationType)
        {
            return $"Type transformation failed for custom transformation \"{typeTransformationType.FullName}\" on target type \"{targetType.FullName}\".";
        }
    }
}