using System;

namespace CoreUi.Razor.Data
{
    public interface IEntityCache
    {
        T GetOrRead<T>(string path, Func<T> readToken);
        void Update<T>(string path, T tokenData);
    }
}