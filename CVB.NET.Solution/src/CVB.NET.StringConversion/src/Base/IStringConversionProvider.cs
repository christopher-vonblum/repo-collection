namespace CVB.NET.StringConversion.Base
{
    using System;

    public interface IStringConversionProvider
    {
        bool HasConverter(Type targetType);
        object Deserialize(string value, Type targetType);
        string Serialize(object obj);
    }
}