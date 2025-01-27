namespace CVB.NET.Configuration.Base
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Diagnostics;
    using Debugging.Proxies;

    [DebuggerTypeProxy(typeof (EnumerableDebugView<>))]
    [DebuggerDisplay("Count = {Count}")]
    [Serializable]
    public class ConfigurationElementCollection<TConfigurationElement> : ConfigurationElementCollectionBase,
        IEnumerable<TConfigurationElement> where TConfigurationElement : ConfigurationElementBase, new()
    {
        public TConfigurationElement this[int index]
        {
            get { return (TConfigurationElement)GetInner(index); }
        }

        public new IEnumerator<TConfigurationElement> GetEnumerator()
        {
            int count = Count;

            for (int i = 0; i < count; i++)
            {
                yield return (TConfigurationElement) GetInner(i);
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new TConfigurationElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return element.ToString();
        }
    }
}