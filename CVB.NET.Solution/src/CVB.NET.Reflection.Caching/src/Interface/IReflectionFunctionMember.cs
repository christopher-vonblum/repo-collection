namespace CVB.NET.Reflection.Caching.Interface
{
    using System.Reflection;
    using Cached;

    public interface IReflectionFunctionMember : ISubInfo<CachedType>
    {
        ParameterInfo[] ParameterInfos { get; }
        CachedParameterInfo[] CachedParameterInfos { get; }
        ParameterInfo[] GetParameterInfos();
    }
}