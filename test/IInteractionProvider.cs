using System;
using System.Collections;
using System.Collections.Generic;

namespace CoreUi
{
    public interface IInteractionProvider
    {
        T CreateInputModel<T>();
        object CreateInputModel(Type t);
        bool RequestInput(Type tInput, object template, out object output);
        bool RequestInput<TInput>(TInput template, out TInput value);
        bool RequestInput<TInput>(out TInput value);
        bool RequestInput<TInput>(TInput template);

        TElement RequestDecision<TSource, TElement>(TSource source, Func<TElement, string> getDisplayString)
            where TSource : IEnumerable<TElement>;
        void Messsage(string message);
    }
}