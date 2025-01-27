namespace CVB.NET.Reflection.Caching.Builtin
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Cached;

    internal static class IntegratedWrapperMap
    {
        public static IReadOnlyDictionary<Type, Type> IntegratedWrappers { get; } = new Dictionary<Type, Type>
                                                                                    {
                                                                                        {typeof (Assembly), typeof (CachedAssembly)},
                                                                                        {typeof (TypeInfo), typeof (CachedType)},
                                                                                        {typeof (MemberInfo), typeof (CachedMemberInfo)},
                                                                                        {typeof (MethodBase), typeof (CachedMethodBase)},
                                                                                        {typeof (ConstructorInfo), typeof (CachedConstructorInfo)},
                                                                                        {typeof (MethodInfo), typeof (CachedMethodInfo)},
                                                                                        {typeof (EventInfo), typeof (CachedEventInfo)},
                                                                                        {typeof (PropertyInfo), typeof (CachedPropertyInfo)},
                                                                                        {typeof (FieldInfo), typeof (CachedFieldInfo)},
                                                                                        {typeof (ParameterInfo), typeof (CachedParameterInfo)}
                                                                                    };
    }
}