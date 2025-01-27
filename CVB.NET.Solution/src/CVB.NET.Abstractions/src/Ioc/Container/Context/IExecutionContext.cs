namespace CVB.NET.Abstractions.Ioc.Container.Context
{
    using System;

    public interface IExecutionContext<T, TOuter> where T : TOuter
    {
        TOuter CurrentEnvironment { get; }

        IDisposable EnvironmentSwitch(TOuter newEnvironment);
        IDisposable EnvironmentOverride(TOuter overrideEnvironment);
    }
}