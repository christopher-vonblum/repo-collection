namespace CVB.NET.Abstractions.Ioc.Container.Registration.Extension
{
    using System;

    using CVB.NET.Abstractions.Ioc.Container.Registration.Extension.Base;

    public class ManualInjection : RegistrationExtensionBase
    {
        private readonly Action<object> injectionLambda;

        public ManualInjection(Action<object> inject)
        {
            this.injectionLambda = inject;
        }

        public override void Resolve(IRegistrationInstruction instruction, object instance)
        {
            this.injectionLambda(instance);
        }
    }
}