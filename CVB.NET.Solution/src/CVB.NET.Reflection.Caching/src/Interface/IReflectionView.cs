namespace CVB.NET.Reflection.Caching.Interface
{
    using System.Reflection;
    using Aspect;

    [ReflectionViewAspect]
    public interface IReflectionView : IReflectable
    {
    }

    public interface IReflectionView<TMetaData> : IReflectionView, IReflectable<TMetaData> where TMetaData : ICustomAttributeProvider
    {
    }
}