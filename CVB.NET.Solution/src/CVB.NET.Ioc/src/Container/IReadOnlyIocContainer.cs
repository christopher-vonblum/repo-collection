namespace CVB.NET.Ioc.Container
{
    using System;
    using Model;
    using PostSharp.Patterns.Contracts;

    public interface IReadOnlyIocContainer
    {
        IImplementationConstruction GetDefaultImplementationConstruction([NotNull] Type tInterface);
        IImplementationConstruction GetDefaultSingletonImplementationConstruction([NotNull] Type tInterface);
        IImplementationConstruction GetNamedSingletonImplementationConstruction([NotNull] Type tInterface, [NotEmpty] string singletonKey);

        bool IsImplementationConstructionRegistered([NotNull] Type tInterface);
        bool IsDefaultSingletonConstructionRegistered([NotNull] Type tInterface);
        bool IsNamedSingletonConstructionRegistered([NotNull] Type tInterface, [NotEmpty] string singletonKey);
    }
}