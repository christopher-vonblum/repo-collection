namespace CVB.NET.TextTemplating.Hosted.Host
{
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Exceptions;
    using Microsoft.VisualStudio.TextTemplating;
    using Mono.TextTemplating;
    using NET.Utils.String;

    /// <summary>
    /// The default 
    /// </summary>
    [Serializable]
    internal class T4TemplateEngineHost : TemplateGenerator, ITextTemplatingEngineHost, ITextTemplatingSessionHost
    {
        public string FileExtension { get; private set; } = string.Empty;

        public Encoding FileEncoding { get; private set; } = Encoding.UTF8;

        public T4TemplateEngineHost(string templateFileNameOrPath)
        {
            TemplateFile = ResolvePath(templateFileNameOrPath);
        }


        public string TemplateFile { get; }

        public IList<string> StandardAssemblyReferences { get; } = new[]
                                                                   {
                                                                       typeof (Uri).Assembly.Location,
                                                                       "Mono.TextTemplating",
                                                                       "CVB.NET.TextTemplating"
                                                                   };

        public IList<string> StandardImports { get; } = new[]
                                                        {
                                                            "System",
                                                            "CVB.NET.TextTemplating.Utils"
                                                        };

        public override object GetHostOption(string optionName)
        {
            switch (optionName)
            {
                case "CacheAssemblies":
                    return true;
                default:
                    return base.GetHostOption(optionName);
            }
        }

        public void SetFileExtension(string extension)
        {
            FileExtension = extension;
        }

        public void SetOutputEncoding(Encoding encoding, bool fromOutputDirective)
        {
            FileEncoding = encoding;
        }

        public void LogErrors(CompilerErrorCollection errors)
        {
            if (errors != null && errors.Count > 0)
            {
                throw new T4TemplateCompilationErrorsOccuredException(errors.Cast<CompilerError>().ToList());
            }
        }

        public override AppDomain ProvideTemplatingAppDomain(string content)
        {
            return AppDomain.CreateDomain("Generation Domain");
        }

        /// <summary>
        /// The current Session.
        /// </summary>
        public ITextTemplatingSession Session { get; set; }

        /// <summary>
        /// Create a Session object that can be used to transmit information into a template. The new Session becomes the current Session.
        /// </summary>
        /// <returns>
        /// A new Session
        /// </returns>
        public ITextTemplatingSession CreateSession() => Session = new TextTemplatingSession();

        protected override string ResolveAssemblyReference(string assemblyReference)
        {
            if (assemblyReference.Equals("Microsoft.VisualStudio.TextTemplating.Interfaces.10.0, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"))
            {
                return string.Empty;
            }

            if (File.Exists(assemblyReference))
            {
                return assemblyReference;
            }

            string resolved;

            if (File.Exists(resolved = ResolveFromBinFolder(assemblyReference)))
            {
                return resolved;
            }

            string fileName = GetFileName(assemblyReference);

            if (File.Exists(resolved = ResolvePath(fileName)))
            {
                return resolved;
            }

            if (File.Exists(resolved = base.ResolveAssemblyReference(fileName)))
            {
                return resolved;
            }

            return fileName;
        }

        private string GetFileName(string assemblyReference)
        {
            int indexOfFirstComma = assemblyReference.IndexOf(',');

            string fileName = "";

            if (indexOfFirstComma == -1)
            {
                fileName += assemblyReference;
            }
            else
            {
                fileName += assemblyReference.Substring(0, indexOfFirstComma);
            }

            return fileName + ".dll";
        }

        private string ResolveFromBinFolder(string fileName)
        {
            return PathUtil.GetAssemblyPath().EnsureSuffix("\\") + fileName;
        }

        protected override string ResolvePath(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentNullException(fileName);
            }

            if (File.Exists(fileName))
            {
                return fileName;
            }

            string baseResult = base.ResolvePath(fileName);

            if (File.Exists(baseResult))
            {
                return baseResult;
            }

            throw new FileNotFoundException(fileName);
        }
    }
}