using System.Linq.Expressions;
using Mono.Cecil;

namespace CVB.NET.Rewriting.Compiler.Services.Interfaces.Cecil
{
    public interface ICecilExpressionCompiler
    {
        MethodDefinition CompileToMethodDefinition<TDelegate>(BlockExpression block, params ParameterExpression[] parameters);
    }
}