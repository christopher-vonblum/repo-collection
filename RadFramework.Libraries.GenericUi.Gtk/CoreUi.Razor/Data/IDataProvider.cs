using System.Collections.Generic;
using StackExchange.Redis;

namespace CoreUi.Razor.Data
{
    public interface IDataProvider
    {
        bool HasData(string path);
        T Load<T>(string path);
        void Save<T>(string path, T data);
        IEnumerable<string> QueryChildKeys(string path);
        IEnumerable<string> QueryDescendantKeys(string path);
        IEnumerable<string> QueryVirtualChildKeys(string path);
        IEnumerable<T> QueryChildren<T>(string path);
        IEnumerable<T> QueryDescendants<T>(string path);
        void Delete(string path);
        void DeleteDescendants(string path);
    }

    public interface IUserDataProvider : IDataProvider
    {
        
    }
    
    public class UserDataProvider : DataProvider
    {
        public UserDataProvider(IConnectionMultiplexer redis, IServer server, IEntityCache cache) : base(redis, server, cache)
        {
        }
    }
}