using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CoreUi.Model;
using CoreUi.Proxy.Factory;

namespace CoreUi.Proxy
{
    
    public class EnumerableProxy<TElement> : ObjectProxy, ICollection<TElement>
    {
        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            if (targetMethod.Name == "GetEnumerator")
            {
                if (targetMethod.DeclaringType.GetGenericArguments().Any())
                {
                    return this.EnumerateGeneric();
                }
                
                return this.Enumerate();
            }
            
            return base.Invoke(targetMethod, args);
        }

        public virtual IEnumerator GetEnumerator()
        {
            return Enumerate();
        }
        
        IEnumerator<TElement> IEnumerable<TElement>.GetEnumerator()
        {
            return EnumerateGeneric();
        }

        private IEnumerator<TElement> EnumerateGeneric()
        {
            IEnumerator e = Enumerate();
            while (e.MoveNext())
            {
                yield return (TElement) e.Current;
            }
        }

        protected override object HandleGetter(string propertykey)
        {
            PropertyDefinition propertyDefinition = Object[propertykey];

            if (propertyDefinition == null)
            {
                return null;
            }
            
            object val = Object[propertyDefinition];
            
            bool isSimple = ProxyFactory.IsSimpleField(typeof(TElement));
            
            if (isSimple)
            {
                if (val == null)
                {
                    val = ProxyFactory.CreateProxyOrValue(typeof(TElement));
                }
                return val;
            }

            IObjectProxy ret = (IObjectProxy)ProxyFactory.CreateProxyOrValue(typeof(TElement));

            ret.Object = (IObject)val ?? ret.Object;
            
            return ret;
        }

        private IEnumerator Enumerate()
        {
            foreach (PropertyDefinition property in Object.PropertyDefinitions)
            {
                yield return HandleGetter(property.Name);
            }
        }

        private IEnumerable<TElement> Me()
        {
            IEnumerator e = Enumerate();
            while (e.MoveNext())
            {
                yield return (TElement) e.Current;
            }
        }
        
        public void Add(TElement item)
        {
            int i = Object.PropertyDefinitions.Count();
            PropertyDefinition prop = Object.CreateProperty(
                new PropertyDefinition 
                    {Name = i.ToString(), 
                        ClrType = T.GetGenericArguments()[0],
                        ClrDeclaringType = T});
            Object[prop] = item;
        }

        public void Clear()
        {
            this.Object.Clear();
        }

        public bool Contains(TElement item)
        {
            return Me().Contains(item);
        }

        public void CopyTo(TElement[] array, int arrayIndex)
        {
            throw new NotSupportedException();
        }

        public bool Remove(TElement item)
        {
            throw new NotSupportedException();
        }

        public int Count => Object.PropertyDefinitions.Count();
        public bool IsReadOnly => false;
    }
}