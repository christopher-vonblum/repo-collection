namespace CVB.NET.Rewriting.Compiler.Error
{
    using System;

    [Serializable]
    public struct TextSelection
    {
        public uint StartLine { get; set; }
        public uint StartColumn { get; set; }
        public uint EndLine { get; set; }
        public uint EndColumn { get; set; }
    }
}