namespace CVB.NET.Exceptions
{
    using System;
    using System.Collections.Generic;

    public static class ExceptionExtensions
    {
        public static AggregateException DeepJoinAggregateExceptions(this Exception ex, List<Exception> merged = null)
        {
            merged = merged ?? new List<Exception>();

            AggregateException ag = ex as AggregateException;

            if (ag != null)
            {
                foreach (Exception inner in ag.InnerExceptions)
                {
                    merged.AddRange(inner.DeepJoinAggregateExceptions().InnerExceptions);
                }

                return new AggregateException(merged);
            }

            var singleInner = ex.InnerException as AggregateException;

            if (singleInner != null)
            {
                merged.AddRange(singleInner.DeepJoinAggregateExceptions().InnerExceptions);
            }
            else if (ex.InnerException != null)
            {
                merged.AddRange(ex.InnerException.DeepJoinAggregateExceptions().InnerExceptions);
            }

            return new AggregateException(merged);
        } 
    }
}
