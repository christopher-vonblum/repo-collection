namespace CVB.NET.StringConversion.Base
{
    using System;

    public interface IStringConverter
    {
        Type TargetType { get; }
        object Deserialize(string value);

        string Serialize(object obj);
    }
}