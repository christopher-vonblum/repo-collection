namespace CVB.NET.Reflection.Caching.Interface
{
    using System.Reflection;

    public interface IReflectable
    {
        ICustomAttributeProvider InnerReflectionInfo { get; }
    }

    public interface IReflectable<TReflected> : IReflectable where TReflected : ICustomAttributeProvider
    {
        new TReflected InnerReflectionInfo { get; }
    }
}