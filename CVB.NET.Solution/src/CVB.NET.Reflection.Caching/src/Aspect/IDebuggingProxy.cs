namespace CVB.NET.Reflection.Caching.Aspect
{
    [DebuggingProxyAspect]
    public interface IDebuggingProxy
    {
        object Inner { get; }
    }
}