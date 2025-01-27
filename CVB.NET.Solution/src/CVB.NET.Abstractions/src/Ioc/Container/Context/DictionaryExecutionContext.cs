namespace CVB.NET.Abstractions.Ioc.Container.Context
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class DictionaryExecutionContext<TKey, TValue> : ExecutionContext<Dictionary<TKey, TValue>, IReadOnlyDictionary<TKey, TValue>>
    {
        public override IReadOnlyDictionary<TKey, TValue> CurrentEnvironment
        {
            get
            {
                return base.CurrentEnvironment
                                .Where(kv => kv.Value != null)
                                .ToDictionary(k => k.Key, v => v.Value);
            }
        }

        public override IDisposable EnvironmentOverride(IReadOnlyDictionary<TKey, TValue> overrideEnvironment)
        {
            return base.EnvironmentOverride(this.ImmutableOverride(overrideEnvironment));
        }

        public Dictionary<TKey, TValue> ImmutableOverride(IReadOnlyDictionary<TKey, TValue> overrides)
        {
            Dictionary<TKey, TValue> newDict = new Dictionary<TKey, TValue>();

            foreach (var entry in this.CurrentEnvironment)
            {
                newDict[entry.Key] = entry.Value;
            }

            foreach (var entry in overrides)
            {
                newDict[entry.Key] = entry.Value;
            }

            return newDict;
        }
    }
}