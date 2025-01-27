using System;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CoreUi.Razor.Data
{
    public class EntityCache : IEntityCache
    {
        private MemoryCache cache = new MemoryCache(new MemoryCacheOptions());
        
        public T GetOrRead<T>(string path, Func<T> read)
        {
            var normalizedPath = PathUtil.NormalizePath(path);

            if (cache.TryGetValue(normalizedPath, out JToken j))
            {
                return j.ToObject<T>();
            }

            var o = read();
            
            var token = JToken.FromObject(o, JsonSerializer.CreateDefault(new JsonSerializerSettings(){Converters = JsonConverters.Converters}));

            cache.CreateEntry(normalizedPath).Value = token;
            
            return o;
        }

        public void Update<T>(string path, T data)
        {
            var normalizedPath = PathUtil.NormalizePath(path);

            if (cache.TryGetValue(normalizedPath, out JToken j))
            {
                cache.Remove(normalizedPath);
            }
            
            cache.CreateEntry(normalizedPath).Value = JToken.FromObject(data, JsonSerializer.CreateDefault(new JsonSerializerSettings{Converters = JsonConverters.Converters}));
        }
    }
}