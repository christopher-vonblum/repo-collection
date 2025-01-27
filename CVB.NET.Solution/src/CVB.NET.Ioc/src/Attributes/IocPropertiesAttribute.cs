namespace CVB.NET.Ioc.Attributes
{
    using System;
    using Model;

    public class IocPropertiesAttribute : Attribute
    {
        public InstanceLifeStyle InstanceLifeStyle { get; }

        public IocPropertiesAttribute(InstanceLifeStyle instanceLifeStyle)
        {
            InstanceLifeStyle = instanceLifeStyle;
        }
    }
}