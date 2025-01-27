namespace CVB.NET.Abstractions.Adapters.Tests.res
{
    public class PropertyInjectionTestModel
    {
        public IInjectMe DefaultService { get; set; }

        [NamedDependency]
        public IInjectMe NamedService1 { get; set; }

        [NamedDependency("namedService2")]
        public IInjectMe InjectMeWithDifferentKey { get; set; }
    }
}