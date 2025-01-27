namespace CVB.NET.Abstractions.Ioc.Injection
{
    using System;

    using CVB.NET.Reflection.Caching.Cached;

    public interface IDependencyInjectionHelper
    {
        void ManualInjection<TInjectionTarget>(Func<Type, string, object> resolveDependency, TInjectionTarget injectionTarget, Action<TInjectionTarget> injectionAction);
        TInjectionTarget ManualConstruction<TInjectionTarget>(Func<Type, string, object> resolveDependency, Func<TInjectionTarget> injectionFunc);
        object AutoConstruct(Func<Type, string, object> resolveDependency, CachedType targetType, Func<CachedType, CachedConstructorInfo> chooseConstructor, Func<CachedParameterInfo, string> getDependencyName);
        void AutoInjectProperties(Func<Type, string, object> resolveDependency, object injectionTarget, Func<CachedPropertyInfo, bool> chooseProperties, Func<CachedPropertyInfo, string> getDependencyName);
        void AutoInjectMethods(Func<Type, string, object> resolveDependency, object injectionTarget, Func<CachedMethodInfo, bool> chooseMethods, Func<CachedParameterInfo, string> getDependencyName);
    }
}
