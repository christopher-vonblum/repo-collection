using System;
using System.Reflection;
using CVB.NET.Rewriting.Compiler.CompilationUnits.Tasks.Cecil.Base;
using Mono.Cecil;
using FieldAttributes = Mono.Cecil.FieldAttributes;

namespace ExampleLib1
{
    public class Transformation : Attribute, ITypeTransformationAttribute
    {
        public void ApplyTransformation(Assembly reflectionAssembly, Mono.Cecil.AssemblyDefinition cecilAssembly, Type reflectionType, Mono.Cecil.TypeDefinition cecilType)
        {
            cecilType.Fields.Add(new FieldDefinition("cecil_injected_me", FieldAttributes.Private, cecilAssembly.MainModule.TypeSystem.String));
        }
    }
}
