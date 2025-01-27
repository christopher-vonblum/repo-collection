namespace CVB.NET.Configuration.Pattern
{
    public abstract class ConfigurableBase<TConfiguration> : IConfigurable
    {
        protected TConfiguration Configuration { get; private set; }

        void IConfigurable.Configure(object configuration)
        {
            Configuration = (TConfiguration) configuration;
        }
    }
}