using CVB.NET.Rewriting.Compiler.Services.Interfaces.Cecil;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace CVB.NET.Rewriting.Compiler.Services.Cecil
{
    public class MethodUsage : IMethodUsage
    {
        public MethodDefinition CallingMethod { get; set; }

        public Instruction CallReference { get; set; }
    }
}