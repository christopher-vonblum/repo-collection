namespace CVB.NET.Abstractions.Ioc.Injection.Lambda
{
    using System;

    using CVB.NET.Reflection.Caching.Cached;

    public interface IDependencyInjectionLambdaGenerator
    {
        Func<object> CreateConstructorInjectionLambda(CachedConstructorInfo injectionConstructor, Func<CachedParameterInfo, string> getDependencyName);
        Action<object> CreateMethodInjectionLambda(Type targetType, CachedMethodInfo injectionMethod, Func<CachedParameterInfo, string> getDependencyName);
        Action<object> CreatePropertyInjectionLambda(Type targetType, CachedPropertyInfo[] injectionProperties, Func<CachedPropertyInfo, string> getDependencyName);
        Action<object> CombineMemberInjectionLambdas(Action<object>[] memberInjectionLambdas);
        Func<object> CombineConstructorInjectionAndMemberInjectionLambdas(Func<object> constructorInjectionLambda, Action<object>[] memberInjectionLambdas);
    }
}