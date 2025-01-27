namespace CVB.NET.Configuration.Serialization
{
    using System;

    public interface IStringConverter
    {
        bool CanConvertFromString(Type sourceType);
        object ConvertFromString(string value);
        string ConvertToString(object value);
    }
}