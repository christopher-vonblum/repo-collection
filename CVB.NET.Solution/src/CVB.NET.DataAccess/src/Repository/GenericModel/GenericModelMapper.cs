namespace CVB.NET.DataAccess.Repository.GenericModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using MetaData.Utils;
    using MetaData.Views;
    using Reflection.Caching;
    using Reflection.Caching.Cached;

    public class GenericModelMapper<TOrmTypeView> : IGenericModelMapper<TOrmTypeView> where TOrmTypeView : OrmTypeMetaDataInfoViewBase
    {
        public IGenericModel<TOrmTypeView> MapInstanceToNewGenericModel(object instance, IDictionary<CachedPropertyInfo, object> propertiesDictionary)
        {
            return new GenericModelImplementation<TOrmTypeView>(instance, propertiesDictionary);
        }

        public IGenericModel<TOrmTypeView> MapInstanceToNewGenericModel(object instance, IDictionary<string, object> propertiesDictionary)
        {
            return MapInstanceToNewGenericModel(instance, GetStrongProperties(instance.GetType(), propertiesDictionary));
        }

        public IGenericModel<TOrmTypeView> MapInstanceToNewGenericModel(object instance)
        {
            Dictionary<CachedPropertyInfo, object> properties = new Dictionary<CachedPropertyInfo, object>();

            CachedType modelType = instance.GetType();

            foreach (CachedPropertyInfo property in modelType.Properties)
            {
                object value = property.InnerReflectionInfo.GetValue(instance);

                Type propertyType = property.InnerReflectionInfo.PropertyType;

                if (value == null && !propertyType.IsInterface && ModelBaseTypeUtils.InheritsFromModelRootBaseType(ReflectionCache.Get<TOrmTypeView>(propertyType)))
                {
                    value = ReflectionCache.Get<CachedType>(propertyType).DefaultConstructor.InnerReflectionInfo.Invoke(null);
                }

                properties.Add(property, value);
            }

            return MapInstanceToNewGenericModel(instance, properties);
        }

        public IGenericModel<TOrmTypeView> MapPropertiesToNewGenericModel(Type type, IDictionary<CachedPropertyInfo, object> propertiesDictionary)
        {
            object instance = ((CachedType) type).DefaultConstructor.InnerReflectionInfo.Invoke(null);

            return MapInstanceToNewGenericModel(instance, propertiesDictionary);
        }

        public IGenericModel<TOrmTypeView> MapPropertiesToNewGenericModel(Type type, IDictionary<string, object> propertiesDictionary)
        {
            return MapPropertiesToNewGenericModel(type, GetStrongProperties(type, propertiesDictionary));
        }

        public object MapGenericModelToNewInstance(Type modelType, IDictionary<CachedPropertyInfo, object> propertiesDictionary)
        {
            object instance = ((CachedType) modelType).DefaultConstructor.InnerReflectionInfo.Invoke(null);

            return SetValuesToInstance(instance, propertiesDictionary);
        }

        public object MapGenericModelToNewInstance(Type modelType, IDictionary<string, object> propertiesDictionary)
        {
            return MapGenericModelToNewInstance(modelType, GetStrongProperties(modelType, propertiesDictionary));
        }

        public object MapGenericModelToInstance(object instance, IDictionary<CachedPropertyInfo, object> properties)
        {
            return SetValuesToInstance(instance, properties);
        }

        public object MapGenericModelToInstance(object instance, IDictionary<string, object> properties)
        {
            return MapGenericModelToInstance(instance, GetStrongProperties(instance.GetType(), properties));
        }

        public IDictionary<CachedPropertyInfo, object> GetStrongProperties(CachedType modelType, IDictionary<string, object> propertiesDictionary)
        {
            IDictionary<CachedPropertyInfo, object> strongPropertiesDictionary = new Dictionary<CachedPropertyInfo, object>();

            foreach (KeyValuePair<string, object> propertyPair in propertiesDictionary)
            {
                if (!propertiesDictionary.ContainsKey(propertyPair.Key))
                {
                    continue;
                }

                strongPropertiesDictionary.Add(modelType.Properties.FirstOrDefault(el => el.InnerReflectionInfo.Name.Equals(propertyPair.Key)), propertiesDictionary[propertyPair.Key]);
            }

            return strongPropertiesDictionary;
        }

        private object SetValuesToInstance(object instance, IDictionary<CachedPropertyInfo, object> properties)
        {
            foreach (CachedPropertyInfo property in properties.Keys)
            {
                if (!property.InnerReflectionInfo.CanWrite)
                {
                    continue;
                }

                property.InnerReflectionInfo.SetValue(instance, properties[property]);
            }

            return instance;
        }
    }
}