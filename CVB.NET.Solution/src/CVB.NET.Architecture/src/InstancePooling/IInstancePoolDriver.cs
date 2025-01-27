namespace CVB.NET.Architecture.src.Pooling
{
    using System.Collections.Generic;

    public interface IInstancePoolDriver
    {
        void Renew(IReadOnlyDictionary<string, object> varyInstanceBy);
        void ReleaseAll();
        IReadOnlyDictionary<string, object> GetCurrentVarianceIdentifier();
        object GetCurrentInstance();
        object GetInstance(IReadOnlyDictionary<string, object> varyInstanceBy);
    }
}