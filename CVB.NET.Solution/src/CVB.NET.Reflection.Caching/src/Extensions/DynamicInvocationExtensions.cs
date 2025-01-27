namespace CVB.NET.Reflection.Caching.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Cached;
    using Interface;

    public static class DynamicInvocationExtensions
    {
        public static CachedMethodInfo ChooseMethodOverload(this ICachedType source, Func<CachedParameterInfo, bool> canResolve, Func<CachedMethodInfo, bool> filter = null, bool considerParameterlessCandidate = false)
        {
            return source.Methods.ChooseInvocationTargetOverload(canResolve, filter, considerParameterlessCandidate);
        }

        public static CachedMethodInfo ChooseMethodOverload<T>(this ICachedType source, IReadOnlyDictionary<string, T> arguments, Func<CachedMethodInfo, bool> filter = null, bool considerParameterlessCandidate = false)
        {
            return source.Methods.ChooseInvocationTargetOverload(arguments.ToArgumentDictionary(), filter, considerParameterlessCandidate);
        }

        public static CachedMethodInfo ChooseStaticMethodOverload(this ICachedType source, Func<CachedParameterInfo, bool> canResolve, Func<CachedMethodInfo, bool> filter = null, bool considerParameterlessCandidate = false)
        {
            return source.StaticMethods.ChooseInvocationTargetOverload(canResolve, filter, considerParameterlessCandidate);
        }

        public static CachedMethodInfo ChooseStaticMethodOverload<T>(this ICachedType source, IReadOnlyDictionary<string, T> arguments, Func<CachedMethodInfo, bool> filter = null, bool considerParameterlessCandidate = false)
        {
            return source.StaticMethods.ChooseInvocationTargetOverload(arguments.ToArgumentDictionary(), filter, considerParameterlessCandidate);
        }
        
        public static CachedConstructorInfo ChooseConstructorOverload(this ICachedType source, Func<CachedParameterInfo, bool> canResolve, Func<CachedConstructorInfo, bool> filter = null, bool considerParameterlessCandidate = true)
        {
            return source.Constructors.ChooseInvocationTargetOverload(canResolve, filter, considerParameterlessCandidate);
        }

        public static CachedConstructorInfo ChooseConstructorOverload<T>(this ICachedType source, IReadOnlyDictionary<string, T> arguments, Func<CachedConstructorInfo, bool> filter = null, bool considerParameterlessCandidate = true)
        {
            return source.Constructors.ChooseInvocationTargetOverload(arguments.ToArgumentDictionary(), filter, considerParameterlessCandidate);
        }

        public static TInvocationCandidate ChooseInvocationTargetOverload<TInvocationCandidate>(this IEnumerable<TInvocationCandidate> candidates, Func<CachedParameterInfo, bool> canResolve, Func<TInvocationCandidate, bool> filter = null, bool considerParameterlessCandidate = true, int? maxParameterCount = null) where TInvocationCandidate : class, IReflectionFunctionMember
        {
            TInvocationCandidate parameterlessCandidate = null;

            // If max parameter count is provided filter all candidates with <= parameter count
            var orderedCandidates = 
                    (maxParameterCount.HasValue 
                   ? candidates.Where(c => c.CachedParameterInfos.Length <= maxParameterCount.Value) 
                   : candidates)
                        .OrderByDescending(c => c.CachedParameterInfos.Length).ToArray();

            int bestScore = -1;
            TInvocationCandidate bestMatch = null;

            foreach (TInvocationCandidate candidate in orderedCandidates)
            {
                // candidate should be considered at all?
                if (filter != null && !filter(candidate))
                {
                    continue;
                }

                // No parameters at all?
                if (!candidate.CachedParameterInfos.Any())
                {
                    parameterlessCandidate = candidate;
                    continue;
                }

                // rating of potential target would be higher than the current best rating?
                if (bestMatch != null && candidate.CachedParameterInfos.Length < bestScore)
                {
                    continue;
                }

                Dictionary<CachedParameterInfo, bool> resolvableArgs = candidate.CachedParameterInfos.ToDictionary(p => p, canResolve);

                // candidate is exact match?
                if (candidate.CachedParameterInfos.All(param => resolvableArgs.ContainsKey(param) && resolvableArgs[param]))
                {
                    bestScore = candidate.CachedParameterInfos.Length;
                    bestMatch = candidate;
                }
            }

            // No match found but candidate without parameters found?
            if (bestMatch == null && considerParameterlessCandidate)
            {
                bestMatch = parameterlessCandidate;
            }

            return bestMatch;
        }

        public static TInvocationCandidate ChooseInvocationTargetOverload<TInvocationCandidate>(this IEnumerable<TInvocationCandidate> candidates, IReadOnlyDictionary<string, object> arguments, Func<TInvocationCandidate, bool> filter = null, bool considerParameterlessCandidate = true) where TInvocationCandidate : class, IReflectionFunctionMember
        {
            return ChooseInvocationTargetOverload(candidates, (p) => arguments.ContainsKey(p.InnerReflectionInfo.Name), filter, considerParameterlessCandidate);
        }

        public static Dictionary<CachedParameterInfo, object> GetParameterArgumentMapping<TT>(this IReflectionFunctionMember functionMember, IReadOnlyDictionary<string, TT> weakArguments)
        {
            return functionMember.GetParameterArgumentMapping<object, TT>(weakArguments);
        }

        public static Dictionary<CachedParameterInfo, T> GetParameterArgumentMapping<T, TT>(this IReflectionFunctionMember functionMember, IReadOnlyDictionary<string, TT> weakArguments) where T : class 
        {
            Dictionary<CachedParameterInfo, T> strongArguments =
                new Dictionary<CachedParameterInfo, T>();

            foreach (CachedParameterInfo paramInfo in functionMember.CachedParameterInfos)
            {
                KeyValuePair<string, TT> weakArg = weakArguments.FirstOrDefault(param => param.Key.Equals(paramInfo.InnerReflectionInfo.Name));

                if ((weakArg.Value as T) != null)
                {
                    strongArguments.Add(paramInfo, weakArg.Value as T);
                }
            }

            return strongArguments;
        }

        public static object[] OrderInvocationTargetArguments<T>(this IReflectionFunctionMember functionMember, IReadOnlyDictionary<string, T> arguments)
        {
            return functionMember.OrderInvocationTargetArguments(functionMember.GetParameterArgumentMapping(arguments));
        }

        public static object[] OrderInvocationTargetArguments<T>(this IReflectionFunctionMember functionMember, IReadOnlyDictionary<CachedParameterInfo, T> arguments)
        {
            if (!functionMember.ParameterInfos.Any())
            {
                return null;
            }

            return functionMember
                .CachedParameterInfos
                .Select(parameter => arguments[parameter])
                .Cast<object>()
                .ToArray();
        }

        public static IReadOnlyDictionary<string, object> ToArgumentDictionary<T>(this IReadOnlyDictionary<string, T> source)
        {
            return source.ToDictionary(key => key.Key, val => (object)val.Value);
        }
    }
}
