using Mono.Cecil;
using Mono.Cecil.Cil;

namespace CVB.NET.Rewriting.Compiler.Services.Interfaces.Cecil
{
    public interface IMethodUsage
    {
        MethodDefinition CallingMethod { get; }
        Instruction CallReference { get; }
    }
}