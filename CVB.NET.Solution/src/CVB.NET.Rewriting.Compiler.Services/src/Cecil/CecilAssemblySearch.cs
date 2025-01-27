using System.Collections.Generic;
using CVB.NET.Rewriting.Compiler.Services.Interfaces.Cecil;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace CVB.NET.Rewriting.Compiler.Services.Cecil
{
    public class CecilAssemblySearch : ICecilAssemblySearch
    {
        private IAssemblyQueryProvider queryProvider = new AssemblyQueryProvider();
        
        public IEnumerable<IMethodUsage> FindMethodUsages(AssemblyDefinition targetAssembly, MethodReference method)
        {
            IEnumerable<MethodBody> methodBodies = queryProvider.QueryMethodBodies(targetAssembly);

            string searchFullName = method.FullName;

            foreach (MethodBody body in methodBodies)
            {
                foreach (Instruction instruction in body.Instructions)
                {
                    if (!(instruction.IsCall() || instruction.IsCallVirtual()))
                    {
                        continue;
                    }

                    if (instruction.GetCallInstructionMethod().FullName == searchFullName)
                    {
                        yield return new MethodUsage
                        {
                            CallReference = instruction,
                            CallingMethod = body.Method
                        };
                    }
                }
            }
        }
    }
}