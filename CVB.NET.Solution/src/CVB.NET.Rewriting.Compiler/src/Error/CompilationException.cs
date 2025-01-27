using System.Reflection;
using Unity.Attributes;

namespace CVB.NET.Rewriting.Compiler.Error
{
    using System;
    using System.Collections.Generic;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Text;
    using Mono.Cecil;
    using Services.Interfaces.Cecil;
    using Services.Interfaces.Roslyn;

    [Serializable]
    public abstract class CompilationException : Exception, ICompilationError
    {
        public DocumentReference[] Locations => locations.ToArray();

        public string Category { get; protected set; }
        public string Code { get; protected set; }

        private ICecilAssemblyTransformationContext cecilAssemblyTransformationContext;
        private IRoslynTransformationContext roslynTransformationContext;
        private readonly List<DocumentReference> locations = new List<DocumentReference>();
        private readonly List<(AssemblyDefinition, int)> metadataTokens = new List<(AssemblyDefinition, int)>();

        protected CompilationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        [InjectionMethod]
        public void Inject(ICecilAssemblyTransformationContext cecilAssemblyTransformationContext, IRoslynTransformationContext roslynTransformationContext)
        {
            this.cecilAssemblyTransformationContext = cecilAssemblyTransformationContext;
            this.roslynTransformationContext = roslynTransformationContext;

            ResolveTokensToDocumentReferences();
        }

        private void ResolveTokensToDocumentReferences()
        {
            metadataTokens.ForEach(t => locations.AddRange(GetCodeLocations(t.Item1, t.Item2)));
        }

        protected void AddCodeLocation(Assembly assembly, MemberInfo member)
        {
            metadataTokens.Add((this.cecilAssemblyTransformationContext.GetTransformationAssembly(assembly.GetName()), member.MetadataToken));
        }

        private DocumentReference[] GetCodeLocations(AssemblyDefinition assembly, int metadataToken)
        {
            List<DocumentReference> locations = new List<DocumentReference>();

            IMetadataTokenProvider cecilProviderByToken = assembly.MainModule.LookupToken(metadataToken);

            if (cecilProviderByToken is IMemberDefinition)
            {
                IMemberDefinition memberDefinition = (IMemberDefinition)cecilProviderByToken;

                INamedTypeSymbol type = roslynTransformationContext.Compilation.GetTypeByMetadataName(memberDefinition.FullName);

                foreach (SyntaxReference syntaxReference in type.DeclaringSyntaxReferences)
                {
                    SyntaxTree parentTree = syntaxReference.SyntaxTree;

                    locations.Add(new DocumentReference(
                        parentTree.FilePath,
                        ToTextSelection(
                            syntaxReference.Span,
                            parentTree)));
                }


            }

            return locations.ToArray();
        }

        private TextSelection ToTextSelection(TextSpan span, SyntaxTree sourceTree)
        {
            TextLineCollection lines = sourceTree.GetText().Lines;

            int position = 0,
                startCol = -1,
                startRow = -1,
                stopCol = -1,
                stopRow = -1;

            for (int line = 0; line < lines.Count; line++)
            {
                string lineText = lines[line].Text.ToString();

                int lineLength = lineText.Length;

                if (startCol == -1 && (position + lineLength > span.Start))
                {
                    startCol = span.Start - position;
                    startRow = line;
                }

                if (!(position + lineLength > span.End))
                {
                    stopCol = span.End - position;
                    stopRow = line;

                    break;
                }

                position += lineLength;
            }

            return new TextSelection()
            {
                StartColumn = (uint)startCol,
                StartLine = (uint)startRow,
                EndColumn = (uint)stopCol,
                EndLine = (uint)stopRow
            };
        }
    }
}