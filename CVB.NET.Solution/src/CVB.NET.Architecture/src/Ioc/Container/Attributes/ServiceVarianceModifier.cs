using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVB.NET.Abstractions.src.Ioc.Container.Attributes
{
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = true, Inherited = false)]
    public class ServiceVarianceModifier : Attribute
    {
        public KeyValuePair<string, object> VarianceModifier { get; } 
        public ServiceVarianceModifier(string key, object value)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            VarianceModifier = new KeyValuePair<string, object>(key, value);
        }
    }
}
