using System;
using System.Linq;
using System.Reflection;

namespace ConsoleApp1
{
    public class ObjectProxy : DispatchProxy, IObject, IObjectProxy
    {
        public IObject Object { get; set; }
        
        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            if (targetMethod.Name.StartsWith("get_"))
            {
                return ConvertToReturnValue(targetMethod.ReturnType, Object[GetPropertykey(targetMethod)]);
            }

            if (targetMethod.Name.StartsWith("set_"))
            {
                Object[GetPropertykey(targetMethod)] = ConvertToInternalValue(targetMethod.GetParameters().Single().ParameterType, args[0]);
                return null;
            }

            throw new NotImplementedException(targetMethod.Name);
        }

        private static string GetPropertykey(MethodInfo targetMethod)
        {
            return targetMethod.DeclaringType.FullName + "." + targetMethod.Name.Substring(4);
        }

        public object this[string propertyName]
        {
            get => Object[propertyName];
            set => Object[propertyName] = value;
        }

        public virtual object ConvertToReturnValue(Type propertyType, object value)
        {
            return Object.ConvertToReturnValue(propertyType, value);
        }

        public virtual object ConvertToInternalValue(Type propertyType, object value)
        {
            return Object.ConvertToInternalValue(propertyType, value);
        }
    }
}