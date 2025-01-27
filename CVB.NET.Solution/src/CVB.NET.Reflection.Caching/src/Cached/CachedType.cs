 // ReSharper disable UnassignedGetOnlyAutoProperty

namespace CVB.NET.Reflection.Caching.Cached
{
    using System;
    using System.Collections.Concurrent;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using Aspect;
    using Aspects.MethodBehaviors;
    using Interface;
    using Lookup;
    using PostSharp.Patterns.Contracts;
    using Wrapper;

    //visualizer: http://stackoverflow.com/questions/16593095/how-to-debug-a-debuggertypeproxy-proxy
    [DebuggerTypeProxy(typeof (CachedTypeDebugView))]
    [DebuggerDisplay("{InnerReflectionInfo.FullName}")]
    public sealed class CachedType : ReflectionSubInfoWrapperBase<Type, ICacheableWrapper>, ICachedType, ICachedReflectionView, IReflectionMemberAccessor
    {
        private readonly ConcurrentDictionary<string, object> cachedLookups = new ConcurrentDictionary<string, object>();

        public CachedType([NotNull] Type reflectedType) : base(reflectedType)
        {
        }

        public object GetOrAddLookupValue(string proxyPropertyName, Func<object> getLookupValue)
        {
            return cachedLookups.GetOrAdd(proxyPropertyName, (name) => getLookupValue());
        }

        [UseLookup(typeof (CachedTypeLookups), nameof(CachedTypeLookups.GetTypeArguments))]
        public CachedType[] GenericTypeArguments { get; }

        [UseLookup(typeof (CachedTypeLookups), nameof(CachedTypeLookups.GetGenericTypeDefinition))]
        public CachedType GenericTypeDefinition { get; }

        public bool HasDefaultConstructor => DefaultConstructor != null;

        [UseLookup(typeof(CachedTypeLookups), nameof(CachedTypeLookups.GetDefaultConstructor))]
        public CachedConstructorInfo DefaultConstructor { get; }

        [UseLookup(typeof(CachedTypeLookups), nameof(CachedTypeLookups.GetPublicConstructors))]
        public CachedConstructorInfo[] Constructors { get; }

        [UseLookup(typeof (CachedTypeLookups), nameof(CachedTypeLookups.GetPublicImplementedProperties))]
        public CachedPropertyInfo[] Properties { get; }

        [UseLookup(typeof (CachedTypeLookups), nameof(CachedTypeLookups.GetPublicFields))]
        public CachedFieldInfo[] Fields { get; }

        [UseLookup(typeof (CachedTypeLookups), nameof(CachedTypeLookups.GetPublicImplementedMethods))]
        public CachedMethodInfo[] Methods { get; }

        [UseLookup(typeof (CachedTypeLookups), nameof(CachedTypeLookups.GetPublicImplementedInterfaces))]
        public CachedInterfaceType[] Interfaces { get; }

        [UseLookup(typeof (CachedTypeLookups), nameof(CachedTypeLookups.GetPublicImplementedEvents))]
        public CachedEventInfo[] Events { get; }

        [UseLookup(typeof (CachedTypeLookups), nameof(CachedTypeLookups.GetPublicStaticMethods))]
        public CachedMethodInfo[] StaticMethods { get; }

        public override string GetCacheKeyIdentifier()
        {
            return "[Type]:" + GetGenericTypeIdentityString(InnerReflectionInfo);
        }

        internal static string GetGenericTypeIdentityString([NotNull] Type type)
        {
            string cacheKey = type.MetadataToken.ToString();

            if (!type.IsGenericType)
            {
                return cacheKey;
            }

            var typeArgs = type.GetGenericArguments();

            cacheKey += '[';

            if (!type.IsGenericTypeDefinition)
            {
                // ReSharper disable once LoopCanBeConvertedToQuery
                foreach (Type argument in typeArgs)
                {
                    cacheKey += GetGenericTypeIdentityString(argument) + "|";
                }
            }
            else
            {
                foreach (Type argument in typeArgs)
                {
                    cacheKey += argument.Name + "|";
                }
            }

            cacheKey += ']';

            return cacheKey;
        }

        protected override ICacheableWrapper GetDeclaringReflectionInfo()
        {
            return (ICacheableWrapper)(CachedType)InnerReflectionInfo.DeclaringType ?? (CachedAssembly)InnerReflectionInfo.Assembly;
        }

        [TrierDoerAspect]
        public static implicit operator Type(CachedType cachedType) => cachedType.InnerReflectionInfo;

        [TrierDoerAspect]
        public static implicit operator CachedType(Type reflectedType) => ReflectionCache.Get<CachedType>(reflectedType);

        public class CachedTypeDebugView : IDebuggingProxy, ICachedType
        {
            public CachedTypeDebugView(ICachedType cachedType)
            {
                Inner = cachedType;
            }

            public ICustomAttributeProvider InnerReflectionInfo { get; }
            public Attribute[] Attributes { get; }
            public Attribute[] InheritedAttributes { get; }
            public CachedType[] GenericTypeArguments { get; }
            public CachedType GenericTypeDefinition { get; }
            public bool HasDefaultConstructor { get; }
            public CachedConstructorInfo DefaultConstructor { get; }
            public CachedConstructorInfo[] Constructors { get; }
            public CachedPropertyInfo[] Properties { get; }
            public CachedFieldInfo[] Fields { get; }
            public CachedMethodInfo[] Methods { get; }
            public CachedInterfaceType[] Interfaces { get; }
            public CachedEventInfo[] Events { get; }
            public CachedMethodInfo[] StaticMethods { get; }

            public ICacheableWrapper Parent { get; }
            ICacheableWrapper ISubInfo.DeclaringReflectionInfo => Parent;

            public string GetCacheKeyIdentifier()
            {
                throw new NotImplementedException();
            }

            public string GetCacheKey()
            {
                throw new NotImplementedException();
            }

            public string GetPropertyCacheKey(string property)
            {
                throw new NotImplementedException();
            }

            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            public object Inner { get; }

            public string GetChildCacheKeyIdentifier(ICacheableWrapper child)
            {
                throw new NotImplementedException();
            }
        }
    }
}