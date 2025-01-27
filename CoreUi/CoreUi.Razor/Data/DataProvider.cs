using System;
using System.Collections.Generic;
using System.Linq;
using CoreUi.Razor.Serialization;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace CoreUi.Razor.Data
{
    public class DataProvider : IDataProvider
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly IServer _server;
        private readonly IEntityCache _cache;

        public DataProvider(IConnectionMultiplexer redis, IServer server, IEntityCache cache)
        {
            _redis = redis;
            _server = server;
            _cache = cache;
        }
        
        public bool HasData(string path)
        {
            var db = _redis.GetDatabase();

            return db.KeyExists(PathUtil.NormalizePath(path));
        }

        public T Load<T>(string path)
        {
            if (!HasData(path))
            {
                throw new InvalidOperationException("Path has no data " + path);
            }

            var normalizedPath = PathUtil.NormalizePath(path);

            return _cache.GetOrRead<T>(normalizedPath, () => JsonConvert.DeserializeObject<T>(_redis.GetDatabase().StringGet(normalizedPath), new NewtonsoftSerializeLinqAdapter()));
        }

        public void Save<T>(string path, T data)
        {
            _redis.GetDatabase().StringSet(PathUtil.NormalizePath(path), JsonConvert.SerializeObject(data, new NewtonsoftSerializeLinqAdapter()));
            _cache.Update(path, data);
        }

        public IEnumerable<string> QueryChildKeys(string path)
        {
            string normalizedPath = PathUtil.NormalizePath(path);

            int childLevel = normalizedPath.Count(c => c == '/') + 1;

            foreach (var item in QueryDescendantKeys(path).Where(p => p.ToString().Count(c => c == '/') == childLevel))
            {
                yield return item;
            }
        }

        public IEnumerable<string> QueryDescendantKeys(string path)
        {
            var descendantSelector = PathUtil.NormalizePath(path) + "/*";
            
            var items = _server.Keys(pattern:descendantSelector);

            foreach (var item in items)
            {
                yield return item;
            }
        }

        public IEnumerable<string> QueryVirtualChildKeys(string path)
        {
            return QueryChildKeys(path).Select(GetLastSegment).Distinct();
        }

        private string GetLastSegment(string path)
        {
            int segSize = 0;
            foreach (char c in path.Reverse())
            {
                if (c == '/')
                {
                    break;
                }

                segSize++;
            }

            return path.Substring(path.Length - segSize, segSize);
        }
        
        public IEnumerable<T> QueryChildren<T>(string path)
        {
            foreach(var item in QueryChildKeys(path))
            {
                yield return Load<T>(path);
            }
        }
        public IEnumerable<T> QueryDescendants<T>(string path)
        {
            foreach (var item in QueryDescendantKeys(path))
            {
                yield return Load<T>(item);
            }
        }

        public void Delete(string path)
        {
            _redis.GetDatabase().KeyDelete(PathUtil.NormalizePath(path));
        }

        public void DeleteDescendants(string path)
        {
            var descendantSelector = PathUtil.NormalizePath(path) + "/*";
            foreach (var redisKey in _server.Keys(pattern: descendantSelector))
            {
                _redis.GetDatabase().KeyDelete(redisKey);
            }
        }
    }
}