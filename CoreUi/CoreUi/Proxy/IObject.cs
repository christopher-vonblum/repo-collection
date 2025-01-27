using System;
using System.Collections.Generic;
using CoreUi.Model;
using CoreUi.Serialization;

namespace CoreUi.Proxy
{
    public interface IObject
    {
        /* bool IsActivated { get; }
        void Activate();
        void Flush();
        void Deactivate();*/
        IEnumerable<PropertyDefinition> PropertyDefinitions { get; }
        IObject Clone();
        PropertyDefinition CreateProperty(PropertyDefinition propertyDefinition);
        void RemoveProperty(string propertyName);
        object this[PropertyDefinition definition] { get; set; }
        PropertyDefinition this[string propertyName] { get; }
        void ReplaceWith(IObject o);
        void Clear();
    }
}