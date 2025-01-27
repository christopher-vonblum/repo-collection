namespace CVB.NET.Reflection.Caching.Base
{
    using System;
    using System.Reflection;
    using Interface;
    using PostSharp.Patterns.Contracts;
    using Utils.Array;

    public abstract class CachedAttributeLocationBase : IAttributeLocation, IReflectable
    {
        private ICustomAttributeProvider AttributeProvider { get; }
        private readonly DebuggableLazy<Attribute[]> attributes;
        private readonly DebuggableLazy<Attribute[]> inheritedAttributes;

        protected CachedAttributeLocationBase([NotNull] ICustomAttributeProvider attributeProvider) : this()
        {
            AttributeProvider = attributeProvider;
        }

        protected CachedAttributeLocationBase()
        {
            attributes = new DebuggableLazy<Attribute[]>(GetAttributes);
            inheritedAttributes = new DebuggableLazy<Attribute[]>(GetInheritedAttributes);
        }

        public Attribute[] Attributes => attributes.Value;

        public Attribute[] InheritedAttributes => inheritedAttributes.Value;

        ICustomAttributeProvider IReflectable.InnerReflectionInfo => AttributeProvider;

        protected virtual Attribute[] GetAttributes()
        {
            if (AttributeProvider == null)
            {
                throw new NotImplementedException();
            }

            return Array.ConvertAll(AttributeProvider.GetCustomAttributes(false), item => (Attribute) item);
        }

        protected virtual Attribute[] GetInheritedAttributes()
        {
            if (AttributeProvider == null)
            {
                throw new NotImplementedException();
            }

            return AttributeProvider.GetCustomAttributes(true).CastArray<Attribute>();
        }
    }
}