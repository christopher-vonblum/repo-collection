namespace CVB.NET.Rewriting.Compiler.CompilationUnits.Tasks.PeVerify.Errors
{
    using System;

    public class UndeterminedVerificationErrorTypeException : Exception
    {
        public UndeterminedVerificationErrorTypeException(string errorType) : base(errorType)
        {
        }
    }
}