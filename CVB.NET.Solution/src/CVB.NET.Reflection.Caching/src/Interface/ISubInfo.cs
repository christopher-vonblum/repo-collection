namespace CVB.NET.Reflection.Caching.Interface
{
    public interface ISubInfo : ICacheableWrapper
    {
        ICacheableWrapper DeclaringReflectionInfo { get; }
    }

    public interface ISubInfo<TParent> : ISubInfo where TParent : ICacheableWrapper
    {
        new TParent DeclaringReflectionInfo { get; }
    }
}