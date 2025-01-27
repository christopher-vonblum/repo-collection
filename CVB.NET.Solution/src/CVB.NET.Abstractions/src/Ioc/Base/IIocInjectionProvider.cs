namespace CVB.NET.Abstractions.Ioc.Base
{
    using System;

    using CVB.NET.Reflection.Caching.Cached;

    public interface IIocInjectionProvider
    {
        void ManualInjection<TInjectionTarget>(TInjectionTarget injectionTarget, Action<TInjectionTarget> injectionAction);
        TInjectionTarget ManualConstruction<TInjectionTarget>(Func<TInjectionTarget> injectionFunc);
        object AutoConstruct(CachedType targetType, Func<CachedType, CachedConstructorInfo> chooseConstructor = null, Func<CachedParameterInfo, string> getDependencyName = null);
        void AutoInjectProperties(object injectionTarget, Func<CachedPropertyInfo, bool> chooseProperties = null, Func<CachedPropertyInfo, string> getDependencyName = null);
        void AutoInjectMethods(object injectionTarget, Func<CachedMethodInfo, bool> chooseMethods = null, Func<CachedParameterInfo, string> getDependencyName = null);
    }
}