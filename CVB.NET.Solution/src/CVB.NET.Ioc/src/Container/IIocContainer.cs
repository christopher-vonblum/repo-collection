namespace CVB.NET.Ioc.Container
{
    using System;
    using Model;
    using PostSharp.Patterns.Contracts;

    public interface IIocContainer : IReadOnlyIocContainer
    {
        void SetDefaultImplementationConstruction([NotNull] Type tInterface, [NotNull] IImplementationConstruction construction);

        void SetDefaultSingletonImplementationConstruction([NotNull] Type tInterface, [NotNull] IImplementationConstruction construction);

        void SetNamedSingletonImplementationConstruction([NotNull] Type tInterface, [NotEmpty] string singletonKey, [NotNull] IImplementationConstruction construction);
    }
}