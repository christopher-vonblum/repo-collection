namespace CVB.NET.Reflection.Caching.Lookup
{
    using System;
    using System.Linq;
    using System.Reflection;

    public static class CachedInterfaceLookups
    {
        public static Func<Type, PropertyInfo[]> GetProperties = (type) => type.GetProperties();
        public static Func<Type, EventInfo[]> GetEvents = (type) => type.GetEvents();

        public static Func<Type, MethodInfo[]> GetMethods = (type)
            => type
                .GetMethods()
                .Where(meth => !meth.IsSpecialName)
                .ToArray();

        public static Func<Type, Type[]> GetBaseInterfaces = (type) => type.GetInterfaces();
    }
}