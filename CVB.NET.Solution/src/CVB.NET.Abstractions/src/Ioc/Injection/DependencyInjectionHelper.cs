namespace CVB.NET.Abstractions.Ioc.Injection
{
    using System;
    using System.Collections.Concurrent;
    using System.Linq;

    using CVB.NET.Abstractions.Ioc.Injection.Lambda;
    using CVB.NET.Abstractions.Ioc.Injection.Parameter;
    using CVB.NET.Reflection.Caching;
    using CVB.NET.Reflection.Caching.Cached;

    public class DependencyInjectionHelper : IDependencyInjectionHelper
    {
        private IDependencyInjectionLambdaGenerator lambdaHelper;

        private ConcurrentDictionary<Tuple<CachedConstructorInfo, RuntimeMethodHandle, RuntimeMethodHandle>, Func<object>> autoConstructorLambdaCache = new ConcurrentDictionary<Tuple<CachedConstructorInfo, RuntimeMethodHandle, RuntimeMethodHandle>, Func<object>>();


        private ConcurrentDictionary<Tuple<CachedType, RuntimeMethodHandle, RuntimeMethodHandle>, Action<object>> autoLambdaCache = new ConcurrentDictionary<Tuple<CachedType, RuntimeMethodHandle, RuntimeMethodHandle>, Action<object>>();

        private ConcurrentDictionary<Tuple<CachedMethodInfo, RuntimeMethodHandle>, Action<object>> autoMethodLambdaCache = new ConcurrentDictionary<Tuple<CachedMethodInfo, RuntimeMethodHandle>, Action<object>>();
        private ConcurrentDictionary<Tuple<CachedType, RuntimeMethodHandle, RuntimeMethodHandle>, Action<object>> autoPropertyLambdaCache = new ConcurrentDictionary<Tuple<CachedType, RuntimeMethodHandle, RuntimeMethodHandle>, Action<object>>();

        public DependencyInjectionHelper(IDependencyInjectionLambdaGenerator lambdaHelper)
        {
            this.lambdaHelper = lambdaHelper;
        }

        public object AutoConstruct(Func<Type, string, object> resolveDependency, CachedType targetType, Func<CachedType, CachedConstructorInfo> chooseConstructor, Func<CachedParameterInfo, string> getDependencyName)
        {
            return this.ManualConstruction(resolveDependency, this.GetAutoConstructorInjectionLambda(targetType, chooseConstructor, getDependencyName));
        }

        private Func<object> GetAutoConstructorInjectionLambda(CachedType targetType, Func<CachedType, CachedConstructorInfo> chooseConstructor, Func<CachedParameterInfo, string> getDependencyName)
        {
            return this.autoConstructorLambdaCache.GetOrAdd(new Tuple<CachedConstructorInfo, RuntimeMethodHandle, RuntimeMethodHandle>(chooseConstructor(targetType), chooseConstructor.Method.MethodHandle, getDependencyName.Method.MethodHandle), () => this.lambdaHelper.CreateConstructorInjectionLambda(chooseConstructor(targetType), getDependencyName)());
        } 

        public void AutoInjectProperties(Func<Type, string, object> resolveDependency, object injectionTarget, Func<CachedPropertyInfo, bool> chooseProperties, Func<CachedPropertyInfo, string> getDependencyName)
        {
            Action<object> injectionLambda = this.autoPropertyLambdaCache.GetOrAdd(new Tuple<CachedType, RuntimeMethodHandle, RuntimeMethodHandle>(
                injectionTarget.GetType(), chooseProperties.Method.MethodHandle, getDependencyName.Method.MethodHandle),
                o => { return this.CreateAutoPropertyInjectionLambda(injectionTarget, chooseProperties, getDependencyName); });

            if (injectionLambda == null)
            {
                return;
            }

            this.ManualInjection(resolveDependency, injectionTarget, injectionLambda);
        }

        private Action<object> CreateAutoPropertyInjectionLambda(object injectionTarget, Func<CachedPropertyInfo, bool> chooseProperties, Func<CachedPropertyInfo, string> getDependencyName)
        {
            CachedType injectionTargetType = injectionTarget.GetType();

            CachedPropertyInfo[] props = injectionTargetType
                                            .Properties
                                            .Where(chooseProperties)
                                            .ToArray();

            return this.lambdaHelper.CreatePropertyInjectionLambda(injectionTargetType, props, getDependencyName);
        }

        public void AutoInjectMethods(Func<Type, string, object> resolveDependency, object injectionTarget, Func<CachedMethodInfo, bool> chooseMethods, Func<CachedParameterInfo, string> getDependencyName)
        {
            Action<object> injectionLambda = this.GetCombinedAutoInjectionLambda(injectionTarget, chooseMethods, getDependencyName);

            if (injectionLambda == null)
            {
                return;
            }

            this.ManualInjection(resolveDependency, injectionTarget, injectionLambda);
        }

        private Action<object> GetCombinedAutoInjectionLambda(object injectionTarget, Func<CachedMethodInfo, bool> chooseMethods, Func<CachedParameterInfo, string> getDependencyName)
        {
            return this.autoLambdaCache.GetOrAdd(new Tuple<CachedType, RuntimeMethodHandle, RuntimeMethodHandle>(
                                                injectionTarget.GetType(), chooseMethods.Method.MethodHandle, getDependencyName.Method.MethodHandle),
                                                t => { return this.CombineMethodAutoInjectionLambdas(injectionTarget, chooseMethods, getDependencyName); });
        }

        private Action<object> CombineMethodAutoInjectionLambdas(object injectionTarget, Func<CachedMethodInfo, bool> chooseMethods, Func<CachedParameterInfo, string> getDependencyName)
        {
            Action<object>[] lambdas = ReflectionCache
                                            .Get<CachedType>(injectionTarget.GetType())
                                            .Methods
                                            .Where(chooseMethods)
                                            .Select(m => this.GetMethodInjectionLambda(m, getDependencyName))
                                            .ToArray();

            if (lambdas.Length == 0)
            {
                return null;
            }

            if (lambdas.Length == 1)
            {
                return lambdas.Single();
            }

            return this.lambdaHelper.CombineMemberInjectionLambdas(lambdas);
        }

        private Action<object> GetMethodInjectionLambda(CachedMethodInfo target, Func<CachedParameterInfo, string> getDependencyName)
        {
            return this.autoMethodLambdaCache.GetOrAdd(new Tuple<CachedMethodInfo, RuntimeMethodHandle>(target, getDependencyName.Method.MethodHandle), o => this.lambdaHelper.CreateMethodInjectionLambda(target.InnerReflectionInfo.DeclaringType, o.Item1, getDependencyName));
        }

        public void ManualInjection<TInjectionTarget>(Func<Type, string, object> resolveDependency, TInjectionTarget injectionTarget, Action<TInjectionTarget> injectionAction)
        {
            using (Arg.UseContextualResolver(resolveDependency))
            {
                injectionAction(injectionTarget);
            }
        }

        public TInjectionTarget ManualConstruction<TInjectionTarget>(Func<Type, string, object> resolveDependency, Func<TInjectionTarget> injectionFunc)
        {
            using (Arg.UseContextualResolver(resolveDependency))
            {
                return injectionFunc();
            }
        }
    }
}