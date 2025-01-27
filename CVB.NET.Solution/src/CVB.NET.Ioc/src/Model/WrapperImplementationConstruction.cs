namespace CVB.NET.Ioc.Model
{
    using Reflection.Caching.Cached;

    public class WrapperImplementationConstruction : IImplementationConstruction
    {
        public InstanceLifeStyle InstanceLifeStyle { get; set; }
        public CachedType Type => Instance.GetType();

        private object Instance;

        public WrapperImplementationConstruction(object instance)
        {
            Instance = instance;
        }

        public object CreateInstance()
        {
            return Instance;
        }
    }
}
