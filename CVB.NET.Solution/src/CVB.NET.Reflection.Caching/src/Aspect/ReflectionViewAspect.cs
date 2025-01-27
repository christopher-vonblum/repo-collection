namespace CVB.NET.Reflection.Caching.Aspect
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Cached;
    using Interface;
    using PostSharp.Aspects;
    using PostSharp.Aspects.Advices;
    using PostSharp.Extensibility;

    [MulticastAttributeUsage(MulticastTargets.Class | MulticastTargets.Interface,
        Inheritance = MulticastInheritance.Strict)]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    [Serializable]
    public class ReflectionViewAspect : TypeLevelAspect
    {
        public override bool CompileTimeValidate(Type type)
        {
            // Validate placed use lookup attributes here
            return true;
        }

        [OnLocationGetValueAdvice,
         MulticastPointcut(Targets = MulticastTargets.Property, Attributes = MulticastAttributes.Instance | MulticastAttributes.NonAbstract)]
        public void OnPropertyGet(LocationInterceptionArgs args)
        {
            UseLookupAttribute useLookupAttribute;

            IReflectionView cachedMetaDataView = (IReflectionView) args.Instance;

            if ((useLookupAttribute
                = ReflectionCache
                    .Get<CachedPropertyInfo>(args.Location.PropertyInfo)
                    .Attributes
                    .OfType<UseLookupAttribute>()
                    .FirstOrDefault()) == null)
            {
                args.ProceedGetValue();
                return;
            }

            if (cachedMetaDataView is ICachedReflectionView)
            {
                ICachedReflectionView view = (ICachedReflectionView) cachedMetaDataView;
                args.Value = view.GetOrAddLookupValue(args.LocationName, () => LookupValueInternal(useLookupAttribute, cachedMetaDataView, args.Location.PropertyInfo.PropertyType));
                return;
            }

            args.Value = 
                LookupValueInternal(
                    useLookupAttribute,
                    cachedMetaDataView,
                    args.Location.PropertyInfo.PropertyType);
        }

        private object LookupValueInternal(UseLookupAttribute useLookupAttribute, IReflectionView cachedMetaDataView, Type propertyType)
        {
            object lookupResult = ReflectionCache.GetLookup(useLookupAttribute.LookupMethod, cachedMetaDataView.InnerReflectionInfo);

            if (lookupResult != null)
            {
                Type resultType = lookupResult.GetType();

                if ((lookupResult.GetType() == propertyType || propertyType.IsAssignableFrom(resultType)))
                {
                    return lookupResult;
                }
            }
            else
            {
                return null;
            }

            object converted;

            if (TryConvert(lookupResult, propertyType, out converted))
            {
                return converted;
            }

            return null;
        }

        private bool TryConvert(object lookupResult, Type targetType, out object converted)
        {
            bool isMultiResult = lookupResult is Array || lookupResult is IEnumerable;

            try
            {
                if (isMultiResult)
                {
                    converted = ConvertMultiResult(targetType, lookupResult);
                }
                else
                {
                    if ((targetType.GetInterface(nameof(ICustomAttributeProvider)) != null && lookupResult is IReflectable)
                        || (targetType.GetInterface(nameof(IReflectable)) != null && lookupResult is ICustomAttributeProvider))
                    {
                        converted = ConvertSingleResult(targetType, lookupResult);
                    }
                    else
                    {
                        converted = lookupResult;
                    }
                }

                if (converted == null)
                {
                    return false;
                }

                return true;
            }
            catch
            {
                converted = null;
                return false;
            }
        }

        private object ConvertSingleResult(Type target, object value)
        {
            if ((value is ICustomAttributeProvider))
            {
                return ReflectionCache.Get(target, (ICustomAttributeProvider) value);
            }
            else if (value is IReflectable)
            {
                return ((IReflectable) value).InnerReflectionInfo;
            }

            return value;
        }

        private object ConvertMultiResult(Type target, object value)
        {
            Type innerSourceType = GetInnerValueType(value);
            Type innerTargetType = GetInnerValueType(target);

            List<object> transformedSource;

            // populate intermediate ienumerable
            if (value is Array)
            {
                transformedSource = ((Array) value).Cast<object>().ToList();
            }
            else
            {
                transformedSource = ((IEnumerable<object>) value).ToList();
            }

            if (target.IsArray)
            {
                Array targetArray = Array.CreateInstance(innerTargetType, transformedSource.Count);

                if (!((innerSourceType.GetInterface(nameof(ICustomAttributeProvider)) != null &&
                       innerTargetType.GetInterface(nameof(IReflectable)) != null)
                      ||
                      (innerSourceType.GetInterface(nameof(IReflectable)) != null &&
                       innerTargetType.GetInterface(nameof(ICustomAttributeProvider)) != null)))
                {
                    // Cast array
                    for (int i = targetArray.GetLowerBound(0); i <= targetArray.GetUpperBound(0); i++)
                    {
                        targetArray.SetValue(transformedSource[i], i);
                    }

                    return targetArray;
                }

                // Convert array
                for (int i = targetArray.GetLowerBound(0); i <= targetArray.GetUpperBound(0); i++)
                {
                    object converted = ConvertSingleResult(innerTargetType, transformedSource[i]);

                    targetArray.SetValue(converted, i);
                }

                return targetArray;
            }


            // no inner type change
            if (innerSourceType == innerTargetType)
            {
                return transformedSource;
            }

            // return intermediate ienumerable
            if (!((innerSourceType.GetInterface(nameof(ICustomAttributeProvider)) != null &&
                   innerTargetType.GetInterface(nameof(IReflectable)) != null)
                  ||
                  (innerSourceType.GetInterface(nameof(IReflectable)) != null &&
                   innerTargetType.GetInterface(nameof(ICustomAttributeProvider)) != null)))
            {
                return transformedSource;
            }

            List<object> targetList = new List<object>();


            // convert ienumerable
            foreach (object o in transformedSource)
            {
                targetList.Add(ConvertSingleResult(innerTargetType, o));
            }

            return targetList;
        }

        private Type GetInnerValueType(object iteratable)
        {
            if (iteratable is Array)
            {
                return iteratable.GetType().GetElementType();
            }

            return iteratable
                .GetType()
                .GetInterfaces()
                .Where(t => t.IsGenericType
                            && t.GetGenericTypeDefinition() == typeof (IEnumerable<>))
                .Select(t => t.GetGenericArguments()[0])
                .FirstOrDefault();
        }

        private Type GetInnerValueType(Type type)
        {
            if (type.IsArray)
            {
                return type.GetElementType();
            }

            return type
                .GetInterfaces()
                .Where(t => t.IsGenericType
                            && t.GetGenericTypeDefinition() == typeof (IEnumerable<>))
                .Select(t => t.GetGenericArguments()[0])
                .FirstOrDefault();
        }
    }
}