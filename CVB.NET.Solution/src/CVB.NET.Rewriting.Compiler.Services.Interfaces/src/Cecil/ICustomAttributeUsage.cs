using Mono.Cecil;

namespace CVB.NET.Rewriting.Compiler.Services.Interfaces.Cecil
{
    public interface ICustomAttributeUsage
    {
        CustomAttribute Attribute { get; }
        ICustomAttributeProvider DeclaringAttributeProvider { get; }
    }

    public interface ICustomAttributeUsage<out TAttributeProvider> : ICustomAttributeUsage where TAttributeProvider : ICustomAttributeProvider
    {
        new TAttributeProvider DeclaringAttributeProvider { get; }
    }
}