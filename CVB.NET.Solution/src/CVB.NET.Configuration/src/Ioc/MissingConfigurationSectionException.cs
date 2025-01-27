namespace CVB.NET.Configuration.Ioc
{
    using System;
    using System.Configuration;
    using PostSharp.Patterns.Contracts;

    public class MissingConfigurationSectionException : Exception
    {
        public MissingConfigurationSectionException([NotEmpty] string sectionName)
            : base($@"{ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None).FilePath} does not contain a configuration section named ""{sectionName}""")
        {
        }
    }
}