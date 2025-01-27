using System;
using System.IO;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace RoslynAOPPOC
{
    class Program
    {
        static void Main(string[] args)
        {
            Rewriter rw = new Rewriter();

            CSharpSyntaxTree class1 = (CSharpSyntaxTree)CSharpSyntaxTree.ParseText(File.ReadAllText("/home/anon/Documents/repos/RoslynAOPPOC/Client.cs"));

            var result = rw.Visit(class1.GetRoot());

            string outputPath = "/home/anon/Documents/repos/RoslynAOPPOC/Client.cs";
            
            if(File.Exists(outputPath))
            
            File.WriteAllText(outputPath, result.ToFullString());
        }
    }

    class Rewriter : CSharpSyntaxRewriter
    {
        public override SyntaxNode VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
            node.AddBodyStatements(new StatementSyntax(new CSharpSyntaxNode()));
            
            return base.VisitMethodDeclaration(node);
        }
    }
}