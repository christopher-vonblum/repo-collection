namespace CVB.NET.StringConversion.Base
{
    using System;

    public interface IConfigurableStringConversionProvider
    {
        void AddStringConverter(Type converterType);
    }
}