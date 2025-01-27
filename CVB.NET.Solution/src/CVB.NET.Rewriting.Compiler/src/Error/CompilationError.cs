using System.Linq;

namespace CVB.NET.Rewriting.Compiler.Error
{
    using System;

    [Serializable]
    public class CompilationError : ICompilationError
    {
        public DocumentReference[] Locations { get; }
        public string Category { get; }
        public string Message { get; }
        public string Code { get; }

        public CompilationError(DocumentReference[] locations, string message, string category, string code)
        {
            Locations = locations;
            Message = message;
            Category = category;
            Code = code;
        }

        public static ICompilationError[] FromException(Exception ex)
        {
            if (ex is AggregateException)
            {
                return FromException(((AggregateException)ex).InnerExceptions.ToArray());
            }

            return new[] { ToError(ex) };
        }

        public static ICompilationError[] FromException(params Exception[] ex)
        {
            return ex.Select(ToError).ToArray();
        }

        private static ICompilationError ToError(Exception ex)
        {
            if (ex is ICompilationError)
            {
                return (ICompilationError)ex;
            }

            return new CompilationError(new DocumentReference[0], ex.Message, ex.Source, ex.HResult == 0 ? string.Empty : ex.HResult.ToString());
        }
    }
}