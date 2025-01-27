namespace CVB.NET.Rewriting.Compiler.CompilationUnits.Tasks.PeVerify.Errors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CompilationUnit;
    using Error;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.CodeAnalysis.Text;
    using Mono.Cecil;
    using Services.Interfaces.Cecil;
    using Services.Interfaces.Roslyn;

    [Serializable]
    public class VerificationError : ICompilationError
    {
        private const string IlError = "IL";
        private const string MdError = "MD";
        private const string ErrorHeader = "[XX]: Error: ";
        public ICompilationUnit Unit { get; private set; }

        public MetadataToken MetadataToken { get; private set; }

        private AssemblyDefinition Assembly { get; }
        private IRoslynTransformationContext Roslyn { get; }
        private List<DocumentReference> locations = new List<DocumentReference>();

        public VerificationError(ICompilationUnit unit, string peVerifyMessage, IRoslynTransformationContext roslyn, AssemblyDefinition assembly)
        {
            if (peVerifyMessage.Length == 0)
                throw new ArgumentException(@"No verification peVerifyMessage was given.", nameof(peVerifyMessage));

            string errorType = peVerifyMessage.Substring(
                peVerifyMessage.IndexOf("[", 0, StringComparison.CurrentCultureIgnoreCase) + 1, 2);

            Unit = unit;

            Category = errorType;

            Roslyn = roslyn;
            Assembly = assembly;

            Code = "PeVerify:" + errorType;

            if (string.Equals(errorType, IlError))
            {
                IlLocation(peVerifyMessage);
            }
            else if (string.Equals(errorType, MdError))
            {
                MdLocation(peVerifyMessage);
            }
            else
            {
                throw new UndeterminedVerificationErrorTypeException(errorType);
            }
        }

        public string Message { get; protected set; }
        public string Code { get; }
        public DocumentReference[] Locations => locations.ToArray();
        public string Category { get; protected set; }

        private void MdLocation(string error)
        {
            int errorStartLocation = error.LastIndexOf("[", StringComparison.CurrentCultureIgnoreCase) + 1;
            int errorEndLocation = error.LastIndexOf("]", StringComparison.CurrentCultureIgnoreCase) - 1;

            string token = error.Substring(errorStartLocation, errorEndLocation - errorStartLocation + 1).Trim();

            Message = error.Substring(ErrorHeader.Length).Trim();

            MetadataToken = new MetadataToken(uint.Parse(token));

            GetErrorLocationsFromMetadataToken();
        }

        private void IlLocation(string error)
        {
            int tokenStart = error.IndexOf(" : ", error.LastIndexOf("[", StringComparison.CurrentCultureIgnoreCase) + 1, StringComparison.CurrentCultureIgnoreCase) + 3;
            int tokenEnd = error.LastIndexOf("]", tokenStart, StringComparison.CurrentCultureIgnoreCase) - 1;

            Message = error.Substring(tokenEnd + 2).Trim();

            string token = error.Substring(tokenStart, tokenEnd - tokenStart + 1).Trim();

            MetadataToken = new MetadataToken(uint.Parse(token));

            GetErrorLocationsFromMetadataToken();
        }

        private void GetErrorLocationsFromMetadataToken()
        {
            IMetadataTokenProvider provider = Assembly.MainModule.LookupToken(MetadataToken);

            if (provider is IMemberDefinition)
            {
                IMemberDefinition definition = (IMemberDefinition) provider;

                IEnumerable<SyntaxTree> results = Roslyn
                    .Project
                    .Documents
                    .Select(doc => doc.GetSyntaxTreeAsync().Result);

                foreach (SyntaxTree tree in results)
                {
                    SemanticModel model = Roslyn.Compilation.GetSemanticModel(tree, true);

                    IEnumerable<MemberDeclarationSyntax> declarationSyntax = tree.GetRoot().DescendantNodes().OfType<MemberDeclarationSyntax>();

                    foreach (MemberDeclarationSyntax memberDeclarationSyntax in declarationSyntax)
                    {
                        ISymbol info = model.GetDeclaredSymbol(memberDeclarationSyntax);

                        if (info.MetadataName.Equals(definition.FullName))
                        {
                            locations.Add(
                                new DocumentReference(
                                    tree.FilePath,
                                    ToTextSelection(
                                        memberDeclarationSyntax.FullSpan,
                                        tree)));
                        }
                    }
                }
            }
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
                       StartColumn = (uint) startCol,
                       StartLine = (uint) startRow,
                       EndColumn = (uint) stopCol,
                       EndLine = (uint) stopRow
                   };
        }
    }
}