using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace DataDomain
{
    public class DataEntity : IEntity
    {
        public Dictionary<string, Dictionary<string, object>> Segments { get; set; }
        [BsonId]
        public string Path { get; set; }

        public bool HasSegment<TSegment>()
        {
            return Segments.ContainsKey(typeof(TSegment).FullName);
        }

        public object GetSegment(Type segment)
        {
            throw new NotImplementedException();
        }

        public TSegment GetSegment<TSegment>()
        {
            throw new NotImplementedException();
        }

        public object GetSegment(IClrType segment)
        {
            throw new NotImplementedException();
        }

        public void SetSegment(IClrType segment, object model)
        {
            throw new NotImplementedException();
        }
    }
}