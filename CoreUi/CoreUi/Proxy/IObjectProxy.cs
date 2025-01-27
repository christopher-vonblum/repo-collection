using System;
using System.Reflection;
using CoreUi.Model;

namespace CoreUi.Proxy
{
    public interface IObjectProxy
    {
        PropertyDefinition Property { get; set; }
        IObjectProxy Parent { get; set; }
        Type T { get; set; }
        IObject Object { get; set; }
        object this[PropertyDefinition definition] { get; set; }
    }
}