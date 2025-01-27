using System.Reflection;

namespace CoreUi
{
    internal delegate bool InputValidator(PropertyInfo propertyInfo, object value, IInteractionProvider interactionProvider);
}