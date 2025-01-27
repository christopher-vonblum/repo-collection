using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CoreUi.Model;
using CoreUi.Proxy;
using CoreUi.Proxy.Factory;
using CoreUi.Serialization;
using ZeroFormatter;

namespace CoreUi.Objects
{
    [ZeroFormattable]
    public class O : IObject
    {
        [Index(0)]
        public virtual ILazyDictionary<string, TypedObject> Properties { get; set; } = new Dictionary<string, TypedObject>().AsLazyDictionary();

        public O() { }

        private IContractSerializer serializer { get; } = new ContractSerializer();

        [IgnoreFormat]
        public IEnumerable<PropertyDefinition> PropertyDefinitions => Properties.OfType<KeyValuePair<string, TypedObject>>().Select(v => v.Value.PropertyDefinition);
        
        public IObject Clone()
        {
            return ZeroFormatterSerializer.Deserialize<O>(ZeroFormatterSerializer.Serialize(this));
        }

        public PropertyDefinition CreateProperty(PropertyDefinition propertyDefinition)
        {
            TypedObject t =new TypedObject{PropertyDefinition = propertyDefinition};
            if (propertyDefinition.ClrType.IsValueType)
            {
                t.Data = Serialize(propertyDefinition,Activator.CreateInstance(propertyDefinition.ClrType));
                t.T = propertyDefinition.ClrType.AssemblyQualifiedName;
            }

            Properties.Add(propertyDefinition.Name, t);
            return propertyDefinition;
        }

        [IgnoreFormat]
        public virtual object this[PropertyDefinition propertyDefinition]
        {
            get
            {
                if (Properties.ContainsKey(propertyDefinition.Name))
                {
                    PropertyDefinition def = this[propertyDefinition.Name];
                    
                    return Deserialize(def);
                }
                else
                    return null;
            }
            set
            {
                if (!Properties.ContainsKey(propertyDefinition.Name))
                {
                    throw new NotSupportedException();    
                }
                
                Properties[propertyDefinition.Name].Data = Serialize(propertyDefinition, value);
            }
        }

        public void RemoveProperty(string propertyName)
        {
            Properties.Remove(propertyName);
        }
        
        [IgnoreFormat]
        public PropertyDefinition this[string propertyName] => Properties.ContainsKey(propertyName) ? Properties[propertyName].PropertyDefinition : null;

        public void ReplaceWith(IObject o)
        {
            Properties = ((O) o).Properties;
        }

        public void Clear()
        {
            this.Properties.Clear();
        }

        private object Deserialize(PropertyDefinition definition)
        {
            // property not defined
            if (!Properties.ContainsKey(definition.Name))
            {
                return null;
            }

            // get raw data
            byte[] data = Properties[definition.Name].Data;

            if (data == null)
            {
                if (definition.ClrType.IsValueType)
                {
                    return Activator.CreateInstance(definition.ClrType);
                }

                return null;
            }

            if (ProxyFactory.IsSimpleField(definition.ClrType))
            {
                return ZeroFormatterSerializer.NonGeneric.Deserialize(definition.ClrType, data);
            }

            if (definition.ClrType.IsInterface)
            {
                return FromTyped(data);
            }
            
            throw new InvalidOperationException();
        }

        private static object FromTyped(byte[] data)
        {
            TypedObject o = ZeroFormatterSerializer.Deserialize<TypedObject>(data);

            object res = ZeroFormatterSerializer.NonGeneric.Deserialize(Type.GetType(o.T), o.Data);

            return res;
        }

        private byte[] Serialize(PropertyDefinition property, object value)
        {
            if (value is IObjectProxy o)
            {
                return HandleProxy(property, o);
            }
            
            if (ProxyFactory.IsSimpleField(property.ClrType))
            {
                return ZeroFormatterSerializer.NonGeneric.Serialize(property.ClrType, value);
            }
            
            if (typeof(IEnumerable).IsAssignableFrom(property.ClrType)
                && property.ClrType != typeof(string))
            {
                return HandleEnumerable(property, (IEnumerable)value);
            }
            
            if (value is O o2)
            {
                return HandleObject(property, o2);
            }

            throw new InvalidOperationException();
        }

        private byte[] HandleObject(PropertyDefinition property, O o2)
        {
            return ZeroFormatterSerializer.Serialize(new TypedObject
            {
                Data = ZeroFormatterSerializer.Serialize((O)o2),
                T = typeof(O).AssemblyQualifiedName,
                PropertyDefinition = property
            }); 
        }

        private byte[] HandleEnumerable(PropertyDefinition property, IEnumerable ie)
        {
            Type t = property.ClrType.GetInterface("IEnumerable`1")?.GetGenericArguments()[0];

            if (t == null)
            {
                t = property.ClrType.GetGenericArguments()[0];
            }
            
            int i = 0;
            return HandleObject(property, new O
            {
                Properties = 
                    EnumerateGeneric(ie)
                        .ToDictionary(
                            k => i.ToString(), 
                            v =>
                            {
                                var o = new TypedObject
                                {
                                    Data = ZeroFormatterSerializer.NonGeneric.Serialize(t, v),
                                    PropertyDefinition = new PropertyDefinition
                                    {
                                        ClrType = t,
                                        ClrDeclaringType = property.ClrType,
                                        Name = i.ToString()
                                    }
                                };
                                i++;
                                return o;
                            })
                        .AsLazyDictionary()
            }); 
        }

        private byte[] HandleProxy(PropertyDefinition property, IObjectProxy objectProxy)
        {
            return HandleObject(property, (O) objectProxy.Object);
        }
        private static IEnumerable<object> EnumerateGeneric(IEnumerable source)
        {
            IEnumerator e = source.GetEnumerator();
            while (e.MoveNext())
            {
                yield return e.Current;
            }
        }
    }
}