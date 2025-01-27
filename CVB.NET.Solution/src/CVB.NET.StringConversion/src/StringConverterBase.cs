namespace CVB.NET.StringConversion
{
    using System;
    using Base;

    public abstract class StringConverterBase<TTargetType> : IStringConverter
    {
        public Type TargetType => typeof (TTargetType);

        public string Serialize(object obj)
        {
            return Serialize((TTargetType) obj);
        }

        object IStringConverter.Deserialize(string value)
        {
            return Deserialize(value);
        }

        public abstract TTargetType Deserialize(string value);

        public abstract string Serialize(TTargetType obj);
    }
}