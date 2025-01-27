namespace CVB.NET.Ioc.Container
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using Attributes;
    using Exception;
    using Model;

    public class IocContainer : IIocContainer
    {
        private ConcurrentDictionary<InterfaceDefinition, IImplementationConstruction> InterfaceImplementationMap { get; } =
            new ConcurrentDictionary<InterfaceDefinition, IImplementationConstruction>(new Dictionary<InterfaceDefinition, IImplementationConstruction>());

        public IImplementationConstruction GetDefaultImplementationConstruction(Type tInterface)
        {
            if (!IsImplementationConstructionRegistered(tInterface))
            {
                throw new NoImplementationFoundException(tInterface);
            }

            return InterfaceImplementationMap[new InterfaceDefinition(tInterface)];
        }

        public IImplementationConstruction GetDefaultSingletonImplementationConstruction(Type tInterface)
        {
            if (!IsDefaultSingletonConstructionRegistered(tInterface))
            {
                throw new NoImplementationFoundException(tInterface);
            }

            return InterfaceImplementationMap[new InterfaceDefinition(tInterface) {SingletonKey = string.Empty}];
        }

        public IImplementationConstruction GetNamedSingletonImplementationConstruction(Type tInterface, string singletonKey)
        {
            if (!IsNamedSingletonConstructionRegistered(tInterface, singletonKey))
            {
                throw new NoImplementationFoundException(tInterface);
            }

            return InterfaceImplementationMap[new InterfaceDefinition(tInterface) {SingletonKey = singletonKey}];
        }

        public void SetDefaultImplementationConstruction(Type tInterface, IImplementationConstruction construction)
        {
            if (construction.InstanceLifeStyle == InstanceLifeStyle.Default && construction.Type.Attributes.OfType<IocPropertiesAttribute>().Any())
            {
                construction.InstanceLifeStyle =
                    construction.Type.Attributes.OfType<IocPropertiesAttribute>().FirstOrDefault().InstanceLifeStyle;
            }

            InterfaceImplementationMap[new InterfaceDefinition(tInterface)] = construction;
        }

        public void SetDefaultSingletonImplementationConstruction(Type tInterface, IImplementationConstruction construction)
        {
            if (construction.InstanceLifeStyle == InstanceLifeStyle.Default && construction.Type.Attributes.OfType<IocPropertiesAttribute>().Any())
            {
                construction.InstanceLifeStyle =
                    construction.Type.Attributes.OfType<IocPropertiesAttribute>().FirstOrDefault().InstanceLifeStyle;
            }

            InterfaceImplementationMap[new InterfaceDefinition(tInterface) {SingletonKey = string.Empty}] = construction;
        }

        public void SetNamedSingletonImplementationConstruction(Type tInterface, string singletonKey, IImplementationConstruction construction)
        {
            if (construction.InstanceLifeStyle == InstanceLifeStyle.Default && construction.Type.Attributes.OfType<IocPropertiesAttribute>().Any())
            {
                construction.InstanceLifeStyle =
                    construction.Type.Attributes.OfType<IocPropertiesAttribute>().FirstOrDefault().InstanceLifeStyle;
            }

            InterfaceImplementationMap[new InterfaceDefinition(tInterface) {SingletonKey = singletonKey}] = construction;
        }

        public bool IsImplementationConstructionRegistered(Type tInterface)
        {
            return InterfaceImplementationMap.ContainsKey(new InterfaceDefinition(tInterface));
        }

        public bool IsDefaultSingletonConstructionRegistered(Type tInterface)
        {
            return InterfaceImplementationMap.ContainsKey(new InterfaceDefinition(tInterface) {SingletonKey = string.Empty});
        }

        public bool IsNamedSingletonConstructionRegistered(Type tInterface, string singletonKey)
        {
            return InterfaceImplementationMap.ContainsKey(new InterfaceDefinition(tInterface) {SingletonKey = singletonKey});
        }
    }
}