namespace CVB.NET.Ioc.Model
{
    using System;

    [Flags]
    public enum InstanceLifeStyle
    {
        Default = 0,
        InstancePerDomain = 1 << 1,
        InstancePerThread = 1 << 2,
        InstanceCrossDomain = 1 << 3,
    }
}