namespace CVB.NET.TextTemplating.Hosted.Attributes
{
    using System;
    using PostSharp.Patterns.Contracts;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class TemplatePathAttribute : Attribute
    {
        public string TemplatePath { get; }

        public TemplatePathAttribute([NotEmpty] string templatePath)
        {
            TemplatePath = templatePath;
        }
    }
}