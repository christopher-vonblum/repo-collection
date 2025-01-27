namespace CVB.NET.TextTemplating.Runtime.Base
{
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading;
    using PostSharp.Patterns.Contracts;
    using Reflection.Caching.Cached;

    public abstract class QueryTemplateBase : IT4Template
    {
        /// <summary>
        /// Helper to produce culture-oriented representation of an object as a string
        /// </summary>
        public ToStringInstanceHelper ToStringHelper { get; } = new ToStringInstanceHelper();

        /// <summary>
        /// The error collection for the generation process
        /// </summary>
        public CompilerErrorCollection Errors => errorsField.Value;

        /// <summary>
        /// Gets the current indent we use when adding lines to the output
        /// </summary>
        public string CurrentIndent
        {
            get { return currentIndent.Value; }
            set { currentIndent = new ThreadLocal<string>(() => value); }
        }

        /// <summary>
        /// Current transformation session
        /// </summary>
        public virtual IDictionary<string, object> Session
        {
            get { return session.Value; }
            set { session = new ThreadLocal<IDictionary<string, object>>(() => value); }
        }

        /// <summary>
        /// The string builder that generation-time code is using to assemble generated output
        /// </summary>
        protected StringBuilder GenerationEnvironment
        {
            get { return generationEnvironmentField.Value; }
            set { generationEnvironmentField = new ThreadLocal<StringBuilder>(() => value); }
        }

        private bool EndsWithNewLine
        {
            get { return endsWithNewline.Value; }
            set { endsWithNewline = new ThreadLocal<bool>(() => value); }
        }

        /// <summary>
        /// A list of the lengths of each indent that was added with PushIndent
        /// </summary>
        private List<int> IndentLengths => indentLengthsField.Value;

        private readonly ThreadLocal<CompilerErrorCollection> errorsField = new ThreadLocal<CompilerErrorCollection>(() => new CompilerErrorCollection());
        private readonly ThreadLocal<List<int>> indentLengthsField = new ThreadLocal<List<int>>(() => new List<int>());
        private ThreadLocal<string> currentIndent = new ThreadLocal<string>(() => string.Empty);
        private ThreadLocal<bool> endsWithNewline = new ThreadLocal<bool>(() => false);

        private ThreadLocal<StringBuilder> generationEnvironmentField = new ThreadLocal<StringBuilder>(() => new StringBuilder());
        private ThreadLocal<IDictionary<string, object>> session = new ThreadLocal<IDictionary<string, object>>();

        protected QueryTemplateBase()
        {
        }

        public string Process()
        {
            Session = new Dictionary<string, object>();

            Initialize();

            return TransformText();
        }

        public string Process(IDictionary<string, object> arguments)
        {
            Session = arguments;

            Initialize();

            return TransformText();
        }

        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void Write(string textToAppend)
        {
            // If we're starting off, or if the previous text ended with a newline,
            // we have to append the current indent first.
            if ((GenerationEnvironment.Length == 0)
                || EndsWithNewLine)
            {
                GenerationEnvironment.Append(CurrentIndent);
                EndsWithNewLine = false;
            }
            // Check if the current text ends with a newline
            if (textToAppend.EndsWith(System.Environment.NewLine, System.StringComparison.CurrentCulture))
            {
                EndsWithNewLine = true;
            }
            // This is an optimization. If the current indent is "", then we don't have to do any
            // of the more complex stuff further down.
            if (CurrentIndent.Length == 0)
            {
                GenerationEnvironment.Append(textToAppend);
                return;
            }
            // Everywhere there is a newline in the text, add an indent after it
            textToAppend = textToAppend.Replace(System.Environment.NewLine, System.Environment.NewLine + CurrentIndent);
            // If the text ends with a newline, then we should strip off the indent added at the very end
            // because the appropriate indent will be added when the next time Write() is called
            if (EndsWithNewLine)
            {
                GenerationEnvironment.Append(textToAppend, 0, textToAppend.Length - CurrentIndent.Length);
            }
            else
            {
                GenerationEnvironment.Append(textToAppend);
            }
        }

        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void WriteLine(string textToAppend)
        {
            Write(textToAppend);
            GenerationEnvironment.AppendLine();
            EndsWithNewLine = true;
        }

        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void Write(string format, params object[] args)
        {
            Write(string.Format(System.Globalization.CultureInfo.CurrentCulture, format, args));
        }

        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void WriteLine(string format, params object[] args)
        {
            WriteLine(string.Format(System.Globalization.CultureInfo.CurrentCulture, format, args));
        }

        /// <summary>
        /// Raise an error
        /// </summary>
        public void Error(string message)
        {
            CompilerError error = new CompilerError {ErrorText = message};
            Errors.Add(error);
        }

        /// <summary>
        /// Raise a warning
        /// </summary>
        public void Warning(string message)
        {
            CompilerError error = new CompilerError
                        {
                            ErrorText = message,
                            IsWarning = true
                        };
            Errors.Add(error);
        }

        /// <summary>
        /// Increase the indent
        /// </summary>
        public void PushIndent([NotNull] string indent)
        {
            CurrentIndent = CurrentIndent + indent;

            IndentLengths.Add(indent.Length);
        }

        /// <summary>
        /// Remove the last indent that was added with PushIndent
        /// </summary>
        public string PopIndent()
        {
            string returnValue = "";

            if (IndentLengths.Count <= 0)
            {
                return returnValue;
            }

            int indentLength = IndentLengths[IndentLengths.Count - 1];

            IndentLengths.RemoveAt(IndentLengths.Count - 1);

            if (indentLength <= 0)
            {
                return returnValue;
            }

            returnValue = CurrentIndent.Substring(CurrentIndent.Length - indentLength);

            CurrentIndent = CurrentIndent.Remove(CurrentIndent.Length - indentLength);

            return returnValue;
        }

        /// <summary>
        /// Remove any indentation
        /// </summary>
        public void ClearIndent()
        {
            IndentLengths.Clear();
            CurrentIndent = "";
        }

        public virtual void Initialize()
        {
        }

        public abstract string TransformText();
    }

    public abstract class QueryTemplateBase<TParameterType> : QueryTemplateBase, IT4Template<TParameterType> where TParameterType : class
    {
        private CachedType ParameterType { get; } = typeof (TParameterType);

        protected TParameterType Description { get; private set; }

        public string Process(TParameterType argumentObject)
        {
            Description = argumentObject;

            Dictionary<string, object> arguments = new Dictionary<string, object>();

            foreach (CachedPropertyInfo property in ParameterType.Properties)
            {
                arguments.Add(property.InnerReflectionInfo.Name, property.InnerReflectionInfo.GetValue(argumentObject));
            }

            return base.Process(arguments);
        }
    }
}