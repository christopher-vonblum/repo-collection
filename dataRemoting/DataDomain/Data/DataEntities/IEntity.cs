using System;

namespace DataDomain
{
    public interface IEntity
    {
        string Path { get; set; }
        bool HasSegment<TSegment>();
        TSegment GetSegment<TSegment>();
        object GetSegment(Type segment);
        object GetSegment(IClrType segment);
        void SetSegment(IClrType segment, object model);
    }
}