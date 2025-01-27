namespace CVB.NET.Configuration.Serialization
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.Linq;
    using System.Runtime.Serialization;
    using Base;
    using Ioc;
    using NET.Ioc.Provider;
    using Serializers;

    public class ConfigStringConverter<TPropertyType> : ConfigStringConverter
    {
        public ConfigStringConverter()
        {
            PropertyType = typeof (TPropertyType);
        }
    }

    public class ConfigStringConverter : TypeConverter, IStringConverter
    {
        protected Type PropertyType { get; set; }

        private static Lazy<IIocProvider> configStringSerializerProvider { get; }
            = new Lazy<IIocProvider>(() => new IocProvider(new ConfigStringSerializerContainer()));

        public IIocProvider ConfigStringSerializerProvider => configStringSerializerProvider.Value;

        public ConfigStringConverter(Type propertyType)
        {
            PropertyType = propertyType;
        }

        protected ConfigStringConverter()
        {
        }

        public bool CanConvertFromString(Type sourceType)
        {
            return CanConvertFrom(null, sourceType);
        }

        public new object ConvertFromString(string value)
        {
            return ConvertFrom(null, null, value);
        }

        public new string ConvertToString(object value)
        {
            return (string) ConvertTo(null, null, value, null);
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return typeof (string) == sourceType
                   && ConfigStringSerializerProvider
                       .IocContainer
                       .IsNamedSingletonConstructionRegistered(typeof (IConfigStringSerializer), PropertyType.AssemblyQualifiedName);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            string rawValue = (string) value;

            if (PropertyType == typeof (string))
            {
                return (string)value;
            }

            if (PropertyType.IsEnum)
            {
                return Enum.Parse(PropertyType, rawValue);
            }

            if (PropertyType.IsArray)
            {
                return ParseArray(rawValue, PropertyType.GetElementType());
            }

            if (PropertyType.IsGenericType && PropertyType.GetGenericTypeDefinition() == typeof(Dictionary<,>))
            {
                Type[] genericArgs = PropertyType.GetGenericArguments();

                DictionarySerializer dictSer = new DictionarySerializer(genericArgs[0], genericArgs[1]);

                return dictSer.Deserialize(rawValue);
            }

            IConfigStringSerializer serializer = ConfigStringSerializerProvider
                .GetNamedSingletonInstance<IConfigStringSerializer>(
                    PropertyType.AssemblyQualifiedName);

            object parsedValue = serializer.DynamicDeserialize(rawValue);

            if (parsedValue == null)
            {
                throw new SerializationException("Serializer " + serializer.GetType().FullName + " could not parse input \"" + rawValue + "\".");
            }

            return parsedValue;
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (PropertyType == typeof(string))
            {
                return value;
            }

            if (PropertyType.IsEnum)
            {
                return value.ToString();
            }

            if (PropertyType.IsArray)
            {
                Array array = (Array)value;

                return SerializeArray(array, array.GetType().GetElementType());
            }

            if (PropertyType.IsGenericType && PropertyType.GetGenericTypeDefinition() == typeof(Dictionary<,>))
            {
                Type[] genericArgs = PropertyType.GetGenericArguments();

                DictionarySerializer dictSer = new DictionarySerializer(genericArgs[0], genericArgs[1]);

                return dictSer.Serialize((IDictionary)value);
            }

            if (ConfigStringSerializerProvider.IocContainer.IsNamedSingletonConstructionRegistered(typeof (IConfigStringSerializer), PropertyType.AssemblyQualifiedName))
            {
                return ConfigStringSerializerProvider
                        .GetNamedSingletonInstance<IConfigStringSerializer>(PropertyType.AssemblyQualifiedName)
                        .DynamicSerialize(value);
            }

            return ConfigStringSerializerProvider
                        .GetNamedSingletonInstance<IConfigStringSerializer>(PropertyType.GetGenericTypeDefinition().AssemblyQualifiedName)
                        .DynamicSerialize(value);
        }

        private object ParseArray(string value, Type elementType)
        {
            ConfigStringConverter converter = new ConfigStringConverter(elementType);

            string[] splitted = value.Split('|');

            Array targetArray = Array.CreateInstance(elementType, splitted.Length);

            for (int i = 0; i < splitted.Length; i++)
            {
                targetArray.SetValue(converter.ConvertFromString(splitted[i]), i);
            }

            return targetArray;
        }
        
        private string SerializeArray(Array array, Type elementType)
        {
            if (elementType == typeof (string))
            {
                string[] stringArray = (string[]) array;

                return string.Join("|", stringArray);
            }

            if (!CanConvertFromString(elementType))
            {
                throw new SerializationException($@"Element type ""{elementType.FullName}"" can not be converted.");
            }
            ConfigStringConverter converter = new ConfigStringConverter(elementType);

            List<string> stringResults = (from object o in array select converter.ConvertToString(o)).ToList();

            return string.Join("|", stringResults);
        }
    }
}