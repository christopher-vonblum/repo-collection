namespace CVB.NET.Configuration.Aspects
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Configuration;
    using System.Data.Entity.Design.PluralizationServices;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using Attributes;
    using Base;
    using PostSharp.Aspects;
    using PostSharp.Extensibility;
    using PostSharp.Reflection;
    using Serialization;

    [MulticastAttributeUsage(MulticastTargets.Class | MulticastTargets.Interface,
        TargetMemberAttributes = MulticastAttributes.Public | MulticastAttributes.Instance,
        Inheritance = MulticastInheritance.Multicast)]
    [Serializable]
    [LinesOfCodeAvoided(7)]
    public sealed class ConfigurationPropertyAspectProvider : TypeLevelAspect, IAspectProvider
    {
        private static readonly Type ConfigurationPropertyAttributeType = typeof (ConfigurationPropertyAttribute);

        private static readonly Type ConfigurationCollectionAttributeType = typeof (ConfigurationCollectionAttribute);

        private static readonly Type TypeConverterAttributeType = typeof (TypeConverterAttribute);

        private static readonly ConstructorInfo ConfigurationPropertyAttributeCtor
            = ConfigurationPropertyAttributeType.GetConstructor(GetDefaultBindingFlags(), null, new[] {typeof (string)},
                null);

        private static readonly ConstructorInfo ConfigurationCollectionAttributeCtor
            = ConfigurationCollectionAttributeType.GetConstructor(GetDefaultBindingFlags(), null, new[] {typeof (Type)},
                null);

        private static readonly ConstructorInfo TypeConverterAttributeCtor
            = TypeConverterAttributeType.GetConstructor(GetDefaultBindingFlags(), null, new[] {typeof (Type)},
                null);

        private static readonly Type ConfigStringConverterTypeDefinition =
            typeof (ConfigStringConverter<>);

        private static readonly Type ConfigurationElementCollectionGenericTypeDefinition =
            typeof (ConfigurationElementCollection<>);

        private static readonly PluralizationService PluralizationService = PluralizationService.CreateService(CultureInfo.GetCultureInfo("en-us"));

        private Type TargetType { get; set; }
        private PropertyInfo[] PropertyInfos { get; set; }

        /// <summary>
        /// Provides new aspects.
        /// </summary>
        /// <param name="targetElement">Code element (<see cref="T:System.Reflection.Assembly"/>, <see cref="T:System.Type"/>, 
        ///               <see cref="T:System.Reflection.FieldInfo"/>, <see cref="T:System.Reflection.MethodBase"/>, <see cref="T:System.Reflection.PropertyInfo"/>, <see cref="T:System.Reflection.EventInfo"/>, 
        ///               <see cref="T:System.Reflection.ParameterInfo"/>, or <see cref="T:PostSharp.Reflection.LocationInfo"/>) to which the current aspect has been applied.
        ///             </param>
        /// <returns>
        /// A set of aspect instances.
        /// </returns>
        public IEnumerable<AspectInstance> ProvideAspects(object targetElement)
        {
            List<PropertyInfo> properties = GetValidProperties();

            bool hasDefaultCollection = properties.Count(prop => prop.PropertyType.IsGenericType
                                                                && prop.PropertyType.GetGenericTypeDefinition()
                                                                == ConfigurationElementCollectionGenericTypeDefinition)
                                       == 1;

            foreach (PropertyInfo propertyInfo in properties)
            {
                ObjectConstruction propertyConstruction;

                if ((propertyInfo.PropertyType.IsGenericType
                     && propertyInfo.PropertyType.GetGenericTypeDefinition()
                     == ConfigurationElementCollectionGenericTypeDefinition)
                    && hasDefaultCollection)
                {
                    propertyConstruction = GetPropertyAttributeConstruction(propertyInfo, true);
                }
                else
                {
                    propertyConstruction = GetPropertyAttributeConstruction(propertyInfo, false);
                }

                if (PropertyNeedsTypeConverterAttribute(propertyInfo))
                {
                    yield return
                        ProvideAttributeInstance(propertyInfo, GetTypeConverterAttributeConstruction(propertyInfo));
                }

                if (propertyInfo.PropertyType.IsGenericType
                    && propertyInfo.PropertyType.GetGenericTypeDefinition()
                    == ConfigurationElementCollectionGenericTypeDefinition)
                {
                    if (hasDefaultCollection)
                    {
                        propertyConstruction.NamedArguments[nameof(ConfigurationPropertyAttribute.IsDefaultCollection)]
                            = true;
                    }

                    yield return
                        ProvideAttributeInstance(propertyInfo, GetCollectionAttributeConstruction(propertyInfo));
                }

                if (propertyInfo.IsDefined(typeof (RequiredProperty), true))
                {
                    propertyConstruction.NamedArguments[nameof(ConfigurationPropertyAttribute.IsRequired)] = true;
                }


                if (propertyInfo.IsDefined(typeof (IdentifierPropertyAttribute), true))
                {
                    propertyConstruction.NamedArguments.Add(nameof(ConfigurationPropertyAttribute.IsKey), true);
                    propertyConstruction.NamedArguments[nameof(ConfigurationPropertyAttribute.IsRequired)] = true;
                }

                yield return ProvideAttributeInstance(propertyInfo, propertyConstruction);
            }
        }

        /// <summary>
        /// Method invoked at build time to initialize the instance fields of the current aspect. This method is invoked
        ///               before any other build-time method.
        /// </summary>
        /// <param name="type">Type to which the current aspect is applied</param><param name="aspectInfo">Reserved for future usage.</param>
        public override void CompileTimeInitialize(Type type, AspectInfo aspectInfo)
        {
            TargetType = type;

            PropertyInfos = TargetType.GetProperties(GetDefaultBindingFlags());
        }

        /// <summary>
        /// Method invoked at build time to ensure that the aspect has been applied to the right target.
        /// </summary>
        /// <param name="type">Type to which the aspect has been applied</param>
        /// <returns>
        /// <c>true</c> if the aspect was applied to an acceptable field, otherwise
        ///               <c>false</c>.
        /// </returns>
        public override bool CompileTimeValidate(Type type)
        {
            return typeof (IConfigurationElement).IsAssignableFrom(type);
        }

        private static AspectInstance ProvideAttributeInstance(PropertyInfo propertyInfo,
                                                               ObjectConstruction attributeConstruction)
        {
            return new AspectInstance(propertyInfo, new CustomAttributeIntroductionAspect(attributeConstruction));
        }

        private static ObjectConstruction GetPropertyAttributeConstruction(
            PropertyInfo propertyInfo,
            bool isDefaultCollection)
        {
            string singularPropertyName = FirstCharToLower(PluralizationService.IsSingular(propertyInfo.Name)
                ? propertyInfo.Name
                : PluralizationService.Singularize(propertyInfo.Name));

            string pluralPropertyName = FirstCharToLower(PluralizationService.Pluralize(singularPropertyName));

            ObjectConstruction propertyAttributeConstruction;

            if (isDefaultCollection)
            {
                propertyAttributeConstruction = new ObjectConstruction(ConfigurationPropertyAttributeCtor, string.Empty);
                return propertyAttributeConstruction;
            }

            if (propertyInfo.PropertyType.IsGenericType
                && propertyInfo.PropertyType.GetGenericTypeDefinition()
                == ConfigurationElementCollectionGenericTypeDefinition)
            {
                propertyAttributeConstruction = new ObjectConstruction(
                    ConfigurationPropertyAttributeCtor,
                    pluralPropertyName);
            }
            else
            {
                propertyAttributeConstruction = new ObjectConstruction(
                    ConfigurationPropertyAttributeCtor,
                    FirstCharToLower(propertyInfo.Name));
            }


            return propertyAttributeConstruction;
        }

        private static ObjectConstruction GetCollectionAttributeConstruction(PropertyInfo propertyInfo)
        {
            string singularPropertyName = PluralizationService.IsSingular(propertyInfo.Name)
                ? propertyInfo.Name
                : PluralizationService.Singularize(propertyInfo.Name);

            Type typeParam = propertyInfo.PropertyType.GetGenericArguments().Single();

            ObjectConstruction collectionAttributeConstruction =
                new ObjectConstruction(ConfigurationCollectionAttributeCtor, typeParam);

            collectionAttributeConstruction.NamedArguments[nameof(ConfigurationCollectionAttribute.AddItemName)] =
                FirstCharToLower(singularPropertyName);

            collectionAttributeConstruction.NamedArguments[nameof(ConfigurationCollectionAttribute.ClearItemsName)] =
                "clear";

            collectionAttributeConstruction.NamedArguments[nameof(ConfigurationCollectionAttribute.RemoveItemName)] =
                "remove";

            return collectionAttributeConstruction;
        }

        private static ObjectConstruction GetTypeConverterAttributeConstruction(PropertyInfo propertyInfo)
        {
            return new ObjectConstruction(TypeConverterAttributeCtor, ConfigStringConverterTypeDefinition.MakeGenericType(propertyInfo.PropertyType));
        }

        private static string FirstCharToLower(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }

            return Char.ToLowerInvariant(str[0]) + str.Substring(1);
        }

        private static string FirstCharToUpper(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }

            return Char.ToUpperInvariant(str[0]) + str.Substring(1);
        }

        private List<PropertyInfo> GetValidProperties()
        {
            List<PropertyInfo> properties = new List<PropertyInfo>();

            foreach (PropertyInfo property in PropertyInfos)
            {
                if (!ValidateProperty(property))
                {
                    continue;
                }

                properties.Add(property);
            }

            return properties;
            ;
        }

        private bool PropertyNeedsTypeConverterAttribute(PropertyInfo property)
        {
            return !(typeof (IConfigurationElement).IsAssignableFrom(property.PropertyType)
                     || typeof (string) == property.PropertyType);
        }

        private bool ValidateProperty(PropertyInfo property)
        {
            bool valid = true;

            valid &= property.CanRead;

            valid &= !property.IsDefined(typeof (ConfigurationPropertyAttribute), false);

            valid &= !property.GetIndexParameters().Any();

            valid &= typeof (IConfigurationElement).IsAssignableFrom(property.DeclaringType);

            // TypeConverterAttribute will be rewritten automatically now.

            /*valid &= property.IsDefined(typeof (TypeConverterAttribute))
                  || typeof (IConfigurationElement).IsAssignableFrom(property.PropertyType)
                  || typeof (string) == property.PropertyType;*/

            return valid;
        }

        private static BindingFlags GetDefaultBindingFlags()
        {
            return BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance;
        }
    }
}