using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using CoreUi.Proxy;

namespace CoreUi.Model
{
    public interface IChooseEntryField<TSource, TElement> where TSource : IEnumerable<TElement>
    {
        TSource Source { get; set; }
        
        TElement Selected { get; set; }
    }
}