namespace CVB.NET.Abstractions.Adapters.Tests.res
{
    public class ConstructorInjectionTestModel
    {
        public IInjectMe generalService;
        public IInjectMe namedService1;
        public IInjectMe injectMeWithDifferentKey;

        public ConstructorInjectionTestModel(IInjectMe generalService, [NamedDependency]IInjectMe namedService1, [NamedDependency("namedService2")]IInjectMe injectMeWithDifferentKey)
        {
            this.generalService = generalService;
            this.injectMeWithDifferentKey = injectMeWithDifferentKey;
            this.namedService1 = namedService1;
        }
    }
}