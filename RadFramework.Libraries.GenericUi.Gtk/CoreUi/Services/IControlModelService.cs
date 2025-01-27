using System;
using CoreUi.Model;

namespace CoreUi.Services
{
    public interface IControlModelService
    {
        Type DetermineModelType(PropertyDefinition propertyDefinition);
    }
}