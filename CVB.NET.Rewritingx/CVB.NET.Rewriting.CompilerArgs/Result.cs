using System.Collections.Generic;

namespace CVB.NET.Rewriting.CompilerArgs
{
    public class Result
    {
        public List<string> Errors { get; set; }
        public List<string> Warnings { get; set; }
        public List<string> Messages { get; set; }
    }
}