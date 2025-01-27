namespace CVB.NET.TextTemplating.Hosted.Base
{
    using System.Collections.Generic;
    using PostSharp.Patterns.Contracts;

    public interface ITextTemplatingProvider
    {
        string ProcessTemplate([NotEmpty] string templateFilePath, [NotNull] object transformationModel);

        string ProcessTemplate([NotEmpty] string templateFilePath, [NotNull] IDictionary<string, object> arguments);
    }
}