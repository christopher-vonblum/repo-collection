namespace CVB.NET.Rewriting.Compiler.Error
{
    using System;

    [Serializable]
    public class DocumentReference
    {
        public string Document { get; }
        public TextSelection TextSelection { get; }

        public DocumentReference(string document, TextSelection textSelection)
        {
            Document = document;
            TextSelection = textSelection;
        }
    }
}