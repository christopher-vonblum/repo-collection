using System.Collections.Generic;

namespace CoreUi.Model
{
    public interface ISelectEntriesField<TSource, TElement> where TSource : IEnumerable<TElement>
    {
        TSource Source { get; set; }
        
        IEnumerable<TElement> Selected { get; set; }
    }
}