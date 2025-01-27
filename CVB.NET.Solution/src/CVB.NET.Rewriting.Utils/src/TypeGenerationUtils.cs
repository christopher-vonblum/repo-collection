namespace CVB.NET.Rewriting.Utils
{
    using System.Linq;
    using Mono.Cecil;
    using Mono.Cecil.Cil;

    public static class TypeGenerationUtils
    {
        public static MethodDefinition CreateEmptyConstructor(TypeDefinition declaringType)
        {
            MethodDefinition emptyCtor =
                new MethodDefinition(
                    ".ctor",
                    MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName,
                    declaringType.Module.TypeSystem.Void);

            ILProcessor cm = emptyCtor.Body.GetILProcessor();

            MethodDefinition emptyBaseCtor =
                declaringType.BaseType.Resolve()
                    .Methods.Where(meth => !meth.Parameters.Any() && meth.IsConstructor)
                    .OrderBy(meth => meth.IsPublic)
                    .FirstOrDefault();

            if (emptyBaseCtor == null)
            {
                throw new NoCompatibleBaseConstructorAvailableException();
            }

            cm.Emit(OpCodes.Ldarg_0);
            cm.Emit(OpCodes.Call, declaringType.Module.Import(emptyBaseCtor));
            cm.Emit(OpCodes.Nop);
            cm.Emit(OpCodes.Ret);

            declaringType.Methods.Add(emptyCtor);

            return emptyCtor;
        }
    }
}