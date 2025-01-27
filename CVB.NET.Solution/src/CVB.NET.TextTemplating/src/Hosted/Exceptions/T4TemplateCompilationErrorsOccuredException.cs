namespace CVB.NET.TextTemplating.Hosted.Exceptions
{
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;

    public class T4TemplateCompilationErrorsOccuredException : Exception
    {
        public IList<CompilerError> Errors { get; private set; }

        public T4TemplateCompilationErrorsOccuredException(IList<CompilerError> errors)
        {
            Errors = errors;
        }
    }
}