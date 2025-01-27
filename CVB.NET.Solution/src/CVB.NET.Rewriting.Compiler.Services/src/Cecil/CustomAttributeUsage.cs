using CVB.NET.Rewriting.Compiler.Services.Interfaces.Cecil;
using Mono.Cecil;

namespace CVB.NET.Rewriting.Compiler.Services.Cecil
{
    public class CustomAttributeUsage<T> : ICustomAttributeUsage<T> where T : ICustomAttributeProvider
    {
        public CustomAttribute Attribute { get; set; }
        public T DeclaringAttributeProvider { get; set; }

        ICustomAttributeProvider ICustomAttributeUsage.DeclaringAttributeProvider => DeclaringAttributeProvider;
    }
}