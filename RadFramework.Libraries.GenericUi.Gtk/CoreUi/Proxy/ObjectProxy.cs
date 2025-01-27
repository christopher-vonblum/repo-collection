using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CoreUi.Attributes;
using CoreUi.Model;
using CoreUi.Proxy.Factory;
using CoreUi.Serialization;
using Newtonsoft.Json;

namespace CoreUi.Proxy
{
    public class ObjectProxy : DispatchProxy, IObjectProxy
    {
        [JsonIgnore]
        public PropertyDefinition Property { get; set; }
        
        [JsonIgnore]
        public IObjectProxy Parent { get; set; }
        [JsonIgnore]
        public Type T { get; set; }
        [JsonIgnore]
        public IObject Object { get; set; }
        [JsonIgnore]
        public object this[PropertyDefinition definition]
        {
            get => HandleGetter(definition.Name);
            set => HandleSetter(definition.Name, value);
        }

        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            if (bindings.ContainsKey(targetMethod))
            {
                if (targetMethod.Name.StartsWith("set_"))
                {
                    HandleSetter(GetPropertyKey(targetMethod),bindings[targetMethod](args, this));
                }

                return bindings[targetMethod](args, this);
            }
            
            if (targetMethod.Name.StartsWith("get_"))
            {
                object res = HandleGetter(GetPropertyKey(targetMethod));
                if (res == null && targetMethod.ReturnType.IsValueType)
                {
                    return Activator.CreateInstance(targetMethod.ReturnType);
                }

                return res;
            }

            if (targetMethod.Name.StartsWith("set_"))
            {
                return HandleSetter(GetPropertyKey(targetMethod), args[0]);
            }

            throw new NotImplementedException(targetMethod.Name);
        }

        protected virtual object HandleSetter(string propertyKey, object arg)
        {
            PropertyDefinition propertyDefinition = Object[propertyKey];
            
            if (propertyDefinition == null)
            {
                throw new InvalidOperationException();
            }

            if (!ProxyFactory.IsSimpleField(propertyDefinition.ClrType))
            {
                IObjectProxy proxy = null;

                if (arg is IObjectProxy o)
                {
                    proxy = o;
                }
                else
                {
                    proxy = (IObjectProxy) ProxyFactory.CreateProxyOrValue(propertyDefinition.ClrType);
                    
                    proxy.Parent = this;
                    proxy.Property = propertyDefinition;

                    if (arg != null)
                    {
                        Map(propertyDefinition, arg, proxy);
                    }
                }
                
                proxy.Parent = this;
                proxy.Property = propertyDefinition;
                

                Object[propertyDefinition] = proxy;

                if (Parent != null)
                {
                    Parent[Property] = this;
                }
                
                return null;
            }

            Object[propertyDefinition] = arg;

            if (Parent != null)
            {
                Parent[Property] = this;
            }
            
            return null;
        }

        protected virtual object HandleGetter(string propertykey)
        {
            PropertyDefinition propertyDefinition = Object[propertykey];

            if (propertyDefinition == null)
            {
                return null;
            }

            object res = Object[propertyDefinition];

            if (res is IObject o)
            {
                IObjectProxy proxy = (IObjectProxy) ProxyFactory.CreateProxyOrValue(propertyDefinition.ClrType);

                proxy.Object = o;
                proxy.Parent = this;
                proxy.Property = propertyDefinition;
                res = proxy;
            }

            if (res == null && ProxyFactory.IsSimpleField(propertyDefinition.ClrType))
            {
                res = ProxyFactory.CreateProxyOrValue(propertyDefinition.ClrType);
            }

            return res;
        }
        
        private static string GetPropertyKey(MethodInfo methodInfo)
        {
            return methodInfo.DeclaringType.Name + "." + GetPropertyName(methodInfo);
        }

        private static string GetPropertyName(MethodInfo targetMethod)
        {
            return  targetMethod.Name.Substring(4);
        }

        private static void Map(PropertyDefinition property, object value, IObjectProxy proxy)
        {
            if (value is IEnumerable ie && !(value is string))
            {
                int i = 0;
                foreach (object val in ie)
                {
                    PropertyDefinition def = new PropertyDefinition
                    {
                        Name = i.ToString(),
                        ClrType = property.ClrType.GetGenericArguments()[0],
                        ClrDeclaringType = property.ClrType
                    };

                    proxy.Object.CreateProperty(def);

                    proxy[def] = val;
                    
                    i++;
                }
                
                return;
            }
            
            IEnumerable<PropertyInfo> properties = property.ClrType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Concat(property.ClrType.GetInterfaces().SelectMany(i => i.GetProperties(BindingFlags.Public | BindingFlags.Instance)));

            foreach (PropertyInfo prop in properties)
            {
                object val = prop.GetValue(value);

                if (val == null)
                {
                    prop.SetValue(proxy, val);
                    continue;
                }
                
                if (ProxyFactory.IsSimpleField(prop.PropertyType))
                {
                    prop.SetValue(proxy, val);
                    continue;
                }
                    
                IObjectProxy childProxy = null;

                if (val is IObjectProxy o)
                {
                    childProxy = o;
                }
                else
                {
                    childProxy = (IObjectProxy) ProxyFactory.CreateProxyOrValue(prop.PropertyType);
                        
                    Map(new PropertyDefinition
                    {
                        Name = prop.Name,
                        ClrType = prop.PropertyType,
                        ClrDeclaringType = prop.DeclaringType
                    }, val, childProxy);
                }
                
                prop.SetValue(proxy, childProxy);
            }
        }
        
        private static ConcurrentDictionary<MethodInfo, Func<object[], IObjectProxy, object>> bindings = new ConcurrentDictionary<MethodInfo, Func<object[], IObjectProxy, object>>();

        public static void BindMethod(MethodInfo methodInfo, Func<object[], IObjectProxy, object> implementation)
        {
            bindings[methodInfo] = implementation;
        }
    }
}