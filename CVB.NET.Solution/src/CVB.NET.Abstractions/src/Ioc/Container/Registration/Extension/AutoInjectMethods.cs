namespace CVB.NET.Abstractions.Ioc.Container.Registration.Extension
{
    using System;

    using CVB.NET.Abstractions.Ioc.Container.Registration.Extension.Base;
    using CVB.NET.Reflection.Caching.Cached;

    public class AutoInjectMethods : RegistrationExtensionBase
    {
        public bool ShouldApply { get; }
        public Func<CachedMethodInfo, bool> ChooseMethods { get; }
        public Func<CachedParameterInfo, string> GetParameterName { get; }

        public AutoInjectMethods(bool value, Func<CachedMethodInfo, bool> chooseMethods, Func<CachedParameterInfo, string> getParameterName)
        {
            this.ShouldApply = value;
            this.ChooseMethods = chooseMethods;
            this.GetParameterName = getParameterName;
        }

        public override void Resolve(IRegistrationInstruction instruction, object instance)
        {
            
        }
    }
}