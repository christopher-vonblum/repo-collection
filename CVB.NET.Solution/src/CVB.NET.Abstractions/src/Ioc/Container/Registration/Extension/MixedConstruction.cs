namespace CVB.NET.Abstractions.Ioc.Container.Registration.Extension
{
    using System;

    using CVB.NET.Abstractions.Ioc.Container.Registration.Extension.Base;

    public class MixedConstruction : RegistrationExtensionBase
    {
        public override Func<object> Constructor { get; }

        public MixedConstruction(Func<object> ctor)
        {
            this.Constructor = ctor;
        }
    }
}