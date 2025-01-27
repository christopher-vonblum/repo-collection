using System;

namespace ConsoleApp1
{
    public interface IObject
    {
        object this[string propertyName] { get; set; }
        object ConvertToReturnValue(Type propertyType, object value);
        object ConvertToInternalValue(Type propertyType, object value);
    }
}