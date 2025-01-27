namespace CVB.NET.Ioc.Aspects
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Linq;
    using System.Reflection;
    using Exceptions.Reflection;
    using PostSharp.Aspects;
    using PostSharp.Extensibility;
    using PostSharp.Patterns.Contracts;
    using PostSharp.Reflection;
    using Provider;
    using Reflection.Caching.Cached;

    [Serializable]
    [AttributeUsage(AttributeTargets.Class)]
    [MulticastAttributeUsage(MulticastTargets.Property, PersistMetaData = true, TargetMemberAttributes = MulticastAttributes.Public)]
    public class IocAccessorAspect : LocationInterceptionAspect
    {
        #region Compile Time

        /// <summary>
        /// 
        /// </summary>
        public bool ResolveNamedSingletons { get; }

        /// <summary>
        /// 
        /// </summary>
        public bool ResolveDefaultSingletons { get; }

        /// <summary>
        /// 
        /// </summary>
        public Type TargetIocPropertyDeclaringType { get; private set; }

        /// <summary>
        /// If true always creates a new instance when the location getter for a non-named default implementation is called.
        /// Otherwise creates one instance per getter location.
        /// </summary>
        public bool FactoryMode { get; }

        /// <summary>
        /// The implementation type that implements IIocProvider and acts as a data source for the static properties.
        /// </summary>
        private Type IocProviderType { get; }

        /// <summary>
        /// The constructor arguments to pass when creating the IIocProvider.
        /// </summary>
        private object[] IocProviderCtorArgs { get; }

        public IocAccessorAspect(
            [NotNull] Type iocProviderType,
            bool resolveNamedSingletons = false,
            bool resolveDefaultSingletons = false,
            string defaultInstancePropertyPrefix = "",
            string defaultInstancePropertySuffix = "",
            bool factoryMode = false,
            params object[] iocProviderConstructorParams)
        {
            if (!typeof (IIocProvider).IsAssignableFrom(iocProviderType))
            {
                throw new InvalidOperationException("Invalid adapter type.");
            }

            FactoryMode = factoryMode;

            ResolveDefaultSingletons = resolveDefaultSingletons;

            ResolveNamedSingletons = resolveNamedSingletons;

            IocProviderType = iocProviderType;

            IocProviderCtorArgs = iocProviderConstructorParams;
        }

        public override void CompileTimeInitialize(LocationInfo targetLocation, AspectInfo aspectInfo)
        {
            TargetIocPropertyDeclaringType = targetLocation.PropertyInfo.DeclaringType;
        }

        #endregion Build Time

        #region Run Time

        /// <summary>
        /// Shares one IocProvider per declaring type. Needed because multicast creates one Aspect per property.
        /// </summary>
        private static ConcurrentDictionary<Type, IIocProvider> IocProviderInstances { get; }
            = new ConcurrentDictionary<Type, IIocProvider>();

        /// <summary>
        /// Stores constructed instances per location in a static context. Only used if FactoryMode is false.
        /// </summary>
        private static ConcurrentDictionary<Tuple<Type, string>, object> LocationInstanceCache { get; }
            = new ConcurrentDictionary<Tuple<Type, string>, object>();

        /// <summary>
        /// Backing field for IocProvider.
        /// </summary>
        private IIocProvider iocProvider;

        /// <summary>
        /// IocProvider for the current declaring type.
        /// </summary>
        public IIocProvider IocProvider => iocProvider
                                           ?? (iocProvider
                                               = IocProviderInstances.GetOrAdd(TargetIocPropertyDeclaringType, CreateIocProviderInstance));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public override void OnGetValue(LocationInterceptionArgs args)
        {
            Type propertyType = args.Location.PropertyInfo.PropertyType;

            if (ResolveNamedSingletons
                && IocProvider.IocContainer.IsNamedSingletonConstructionRegistered(propertyType, args.LocationName))
            {
                args.Value = IocProvider.GetNamedSingletonInstance(propertyType, args.LocationName);
                return;
            }

            if (ResolveDefaultSingletons
                && IocProvider.IocContainer.IsDefaultSingletonConstructionRegistered(propertyType))
            {
                args.Value = IocProvider.GetDefaultSingletonInstance(propertyType);
                return;
            }

            if (!IocProvider.IocContainer.IsImplementationConstructionRegistered(propertyType))
            {
                args.ProceedGetValue();
                return;
            }

            if (FactoryMode)
            {
                args.Value = IocProvider.CreateInstance(args.Location.PropertyInfo.PropertyType);
                return;
            }

            args.Value = LocationInstanceCache.GetOrAdd(new Tuple<Type, string>(propertyType, args.LocationName),
                getInstance => IocProvider.CreateInstance(args.Location.PropertyInfo.PropertyType));
        }

        /// <summary>
        /// Creates a instance of a IocProvider for the current declaring type.
        /// </summary>
        /// <param name="declaringType">Neccessary to match the GetOrAdd delegate of ConcurrentDictionary.</param>
        /// <returns>The constructed IIocProvider.</returns>
        private IIocProvider CreateIocProviderInstance(Type declaringType)
        {
            IIocProvider constructedIocProvider;

            CachedType iocProviderType = IocProviderType;

            if (iocProviderType.HasDefaultConstructor && !IocProviderCtorArgs.Any())
            {
                constructedIocProvider
                    = (IIocProvider) iocProviderType
                        .DefaultConstructor
                        .InnerReflectionInfo
                        .Invoke(null);
            }
            else
            {
                List<Type> constructorParameterTypes = IocProviderCtorArgs.ToList().Select(arg => arg.GetType()).ToList();

                CachedConstructorInfo iocProviderCtor
                    = iocProviderType.Constructors
                        .SingleOrDefault(
                            ctor => ctor
                                .CachedParameterInfos.ToList().TrueForAll(param =>
                                    constructorParameterTypes.Contains(param.InnerReflectionInfo.ParameterType)));

                if (iocProviderCtor == null || !iocProviderCtor.ParameterInfos.Any())
                {
                    throw new NoCompatibleConstructorFoundException(iocProviderType.InnerReflectionInfo, IocProviderCtorArgs.Select(arg => arg.ToString()).ToImmutableList());
                }

                try
                {
                    constructedIocProvider
                        = (IIocProvider) iocProviderCtor
                            .InnerReflectionInfo
                            .Invoke(IocProviderCtorArgs);
                }
                catch (TargetInvocationException ex)
                {
                    throw ex.InnerException;
                }
            }

            return constructedIocProvider;
        }

        #endregion Run Time
    }
}