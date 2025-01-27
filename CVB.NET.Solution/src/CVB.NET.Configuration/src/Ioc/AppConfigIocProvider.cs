namespace CVB.NET.Configuration.Ioc
{
    using NET.Ioc.Provider;
    using PostSharp.Patterns.Contracts;

    public class AppConfigIocProvider : IocProvider
    {
        public AppConfigIocProvider([NotEmpty] string appConfigSection) : base(new AppConfigIocContainer(appConfigSection))
        {
        }
    }
}