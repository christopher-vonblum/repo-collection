namespace CVB.NET.Aspects.Validation.BuildTime
{
    using System;
    using System.Linq;
    using PostSharp.Aspects;
    using PostSharp.Extensibility;

    [MulticastAttributeUsage(MulticastTargets.Class | MulticastTargets.Interface,
        Inheritance = MulticastInheritance.Multicast)]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    [Serializable]
    public class RequiresConstructorContract : TypeLevelAspect
    {
        private readonly Type[] parameterTypes;

        public RequiresConstructorContract(params Type[] parameterTypes)
        {
            this.parameterTypes = parameterTypes;
        }

        /// <summary>
        /// Method invoked at build time to ensure that the aspect has been applied to the right target.
        /// </summary>
        /// <param name="type">Type to which the aspect has been applied</param>
        /// <returns>
        /// <c>true</c> if the aspect was applied to an acceptable field, otherwise
        ///               <c>false</c>.
        /// </returns>
        public override bool CompileTimeValidate(Type type)
        {
            if (type.IsInterface)
            {
                return true;
            }

            if (!parameterTypes.Any())
            {
                return type.GetConstructor(Type.EmptyTypes) != null;
            }

            return type.GetConstructor(parameterTypes) != null;
        }
    }
}