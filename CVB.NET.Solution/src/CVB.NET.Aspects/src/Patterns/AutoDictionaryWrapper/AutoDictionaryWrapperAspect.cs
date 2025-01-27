namespace CVB.NET.Aspects.Patterns.AutoDictionaryWrapper
{
    using System;
    using System.Collections.Generic;
    using PostSharp.Aspects;
    using PostSharp.Extensibility;
    using PostSharp.Patterns.Contracts;
    using PostSharp.Reflection;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    [MulticastAttributeUsage(MulticastTargets.Property)]
    [Serializable]
    public class AutoDictionaryWrapperAspect : LocationInterceptionAspect
    {
        private string KeyPrefix { get; }

        private string KeySuffix { get; }

        private bool Throw { get; }

        public AutoDictionaryWrapperAspect([NotNull] string keyPrefix = "", [NotNull] string keySuffix = "", bool @throw = false)
        {
            KeyPrefix = keyPrefix;
            KeySuffix = keySuffix;
            Throw = @throw;
        }

        public override bool CompileTimeValidate(LocationInfo locationInfo)
        {
            return typeof (IAutoDictionaryWrapper).IsAssignableFrom(locationInfo.DeclaringType);
        }

        public override void OnGetValue(LocationInterceptionArgs args)
        {
            if (args.LocationName.Equals(nameof(IAutoDictionaryWrapper.WrappedDictionary)))
            {
                if (args.Value == null)
                {
                    args.Value = typeof (Dictionary<string, object>).GetConstructor(Type.EmptyTypes).Invoke(null);
                }

                args.ProceedGetValue();
                return;
            }

            IAutoDictionaryWrapper dictionaryWrapper = (IAutoDictionaryWrapper) args.Instance;

            string key = GetKey(args.LocationName);

            if (!Throw && !dictionaryWrapper.WrappedDictionary.ContainsKey(key))
            {
                return;
            }

            args.Value = dictionaryWrapper.WrappedDictionary[key];
        }

        public override void OnSetValue(LocationInterceptionArgs args)
        {
            if (args.LocationName.Equals(nameof(IAutoDictionaryWrapper.WrappedDictionary)))
            {
                args.ProceedSetValue();
            }

            IAutoDictionaryWrapper dictionaryWrapper = (IAutoDictionaryWrapper) args.Instance;

            dictionaryWrapper.WrappedDictionary[GetKey(args.LocationName)] = args.Value;
        }

        private string GetKey(string locationName)
        {
            return KeyPrefix + locationName + KeySuffix;
        }
    }
}