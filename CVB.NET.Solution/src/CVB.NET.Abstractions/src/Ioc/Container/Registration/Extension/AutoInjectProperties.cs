namespace CVB.NET.Abstractions.Ioc.Container.Registration.Extension
{
    using System;

    using CVB.NET.Abstractions.Ioc.Container.Registration.Extension.Base;
    using CVB.NET.Reflection.Caching.Cached;

    public class AutoInjectProperties : RegistrationExtensionBase
    {
        private bool enable;
        private Func<CachedPropertyInfo, bool> chooseProperties;
        private Func<CachedPropertyInfo, string> getPropertyName;

        public AutoInjectProperties()
        {
            
        }

        public AutoInjectProperties(bool enable, Func<CachedPropertyInfo, bool> chooseProperties, Func<CachedPropertyInfo, string> getPropertyName)
        {
            this.enable = enable;
            this.chooseProperties = chooseProperties;
            this.getPropertyName = getPropertyName;
        }

        public override void Resolve(IRegistrationInstruction instruction, object instance)
        {
            
        }
    }
}