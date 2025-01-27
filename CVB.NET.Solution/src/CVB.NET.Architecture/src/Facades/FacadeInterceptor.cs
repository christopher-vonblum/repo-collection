using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Castle.DynamicProxy;

using CVB.NET.Abstractions.Ioc;
using CVB.NET.Reflection.Caching.Cached;

namespace CVB.NET.Architecture.Facades
{
    public class FacadeInterceptor : IInterceptor
    {
        private List<int> redirectInvocations { get; }
        private IDependencyResolver facadeDependencyContainer { get; }

        private readonly FacadeProxyGenerator facadeGenerator;

        public FacadeInterceptor(FacadeProxyGenerator facadeGenerator, IDependencyResolver facadeDependencyContainer, Type innerFacadeType)
        {
            this.facadeGenerator = facadeGenerator;
            this.facadeDependencyContainer = facadeDependencyContainer;
            this.redirectInvocations = innerFacadeType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy).Select(m => m.MetadataToken).ToList();
        }

        public void Intercept(IInvocation invocation)
        {
            CachedMethodInfo info = invocation.Method;

            if (this.redirectInvocations != null && this.redirectInvocations.Contains(invocation.MethodInvocationTarget.MetadataToken))
            {
                invocation.Proceed();
                return;
            }

            if (!info.InnerReflectionInfo.Name.StartsWith("get_"))
            {
                invocation.Proceed();
                return;
            }

            string dependencyName = invocation.Method.Name.Substring(4);
            CachedInterfaceType dependencyType = invocation.Method.ReturnType;

            if (info.DeclaringReflectionInfo.Properties.FirstOrDefault(p => p.InnerReflectionInfo.Name.Equals(dependencyName)) == null)
            {
                invocation.Proceed();
                return;
            }

            if (!dependencyType.InnerReflectionInfo.IsInterface)
            {
                invocation.Proceed();
                return;
            }

            if (this.IsFacade(dependencyType))
            {
                invocation.ReturnValue = this.facadeGenerator.Create(dependencyType);
                return;
            }

            invocation.ReturnValue = this.facadeDependencyContainer.Resolve(dependencyType.InnerReflectionInfo, dependencyName);
        }

        private bool IsFacade(CachedInterfaceType facadeType)
        {
            return facadeType.BaseInterfaceTypes.FirstOrDefault(i => i.InnerReflectionInfo == typeof(IFacadeBase)) != null;
        }
    }
}