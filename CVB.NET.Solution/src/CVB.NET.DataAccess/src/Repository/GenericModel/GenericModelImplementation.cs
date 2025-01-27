namespace CVB.NET.DataAccess.Repository.GenericModel
{
    using System;
    using System.Collections.Generic;
    using MetaData.Views;
    using PostSharp.Patterns.Contracts;
    using Reflection.Caching;
    using Reflection.Caching.Cached;

    /// <summary>
    /// Default generic model implementation
    /// </summary>
    public class GenericModelImplementation<TOrmTypeView> : IGenericModel<TOrmTypeView> where TOrmTypeView : OrmTypeMetaDataInfoViewBase
    {
        /// <summary>
        /// The generic model mapper
        /// </summary>
        private static IGenericModelMapper<TOrmTypeView> Mapper;


        private object instance;

        private IDictionary<CachedPropertyInfo, object> properties;

        /// <summary>
        /// ..ctor
        /// </summary>
        static GenericModelImplementation()
        {
            Mapper = new GenericModelMapper<TOrmTypeView>();
        }

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="model">The model instance.</param>
        /// <param name="properties">The model properties.</param>
        public GenericModelImplementation(
            [NotNull] object model,
            [NotNull] IDictionary<CachedPropertyInfo, object> properties = null)
        {
            Type = model.GetType();
            OrmTypeDefinition = ReflectionCache.Get<TOrmTypeView>(Type.InnerReflectionInfo);
            instance = model;

            if (properties == null)
            {
                Properties = GetProperties(model, Type);
                return;
            }

            Properties = properties.MergeLeft(GetProperties(instance, Type));
        }

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="type">The model type.</param>
        /// <param name="properties">The model properties.</param>
        public GenericModelImplementation(
            [NotNull] CachedType type,
            [NotNull] IDictionary<CachedPropertyInfo, object> properties = null)
        {
            Type = type;
            OrmTypeDefinition = ReflectionCache.Get<TOrmTypeView>(Type.InnerReflectionInfo);
            Instance = type.DefaultConstructor.InnerReflectionInfo.Invoke(null);

            if (properties == null)
            {
                Properties = GetProperties(instance, Type);
                return;
            }

            Properties = properties.MergeLeft(GetProperties(instance, Type));
        }

        /// <summary>
        /// CachedType of the model.
        /// </summary>
        public CachedType Type { get; }

        /// <summary>
        /// OrmTypeMetaDataViewBase of the model.
        /// </summary>
        public TOrmTypeView OrmTypeDefinition { get; }

        /// <summary>
        /// The properties of the model.
        /// </summary>
        public IDictionary<CachedPropertyInfo, object> Properties
        {
            get { return properties ?? (properties = new Dictionary<CachedPropertyInfo, object>()); }
            set
            {
                if (value == null)
                {
                    throw new InvalidOperationException("Use IDictionary.Clear() instead.");
                }

                properties = value;
            }
        }

        /// <summary>
        /// The model instance.
        /// </summary>
        public object Instance
        {
            get
            {
                if (instance == null)
                {
                    return instance = Mapper.MapGenericModelToNewInstance(Type, Properties);
                }

                return instance = Mapper.MapGenericModelToInstance(instance, Properties);
            }
            set
            {
                if (value == null
                    || !Type.InnerReflectionInfo.IsInstanceOfType(value))
                {
                    throw new InvalidOperationException();
                }

                instance = value;
            }
        }

        public OrmTypeMetaDataInfoViewBase MetaDataInfoViewBase => OrmTypeDefinition;

        private IDictionary<CachedPropertyInfo, object> GetProperties(object model, CachedType type)
        {
            IDictionary<CachedPropertyInfo, object> properties = new Dictionary<CachedPropertyInfo, object>();

            foreach (CachedPropertyInfo property in type.Properties)
            {
                properties.Add(property, property.InnerReflectionInfo.GetValue(model));
            }

            return this.properties;
        }
    }
}