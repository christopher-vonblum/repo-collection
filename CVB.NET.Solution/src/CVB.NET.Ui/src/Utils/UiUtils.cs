namespace CVB.NET.Ui.Utils
{
    public static class UiUtils
    {
        /*public static string GetUiMaskString(object obj)
        {
            var stringObj = obj as string;

            if (stringObj != null)
            {
                return stringObj;
            }

            CachedPropertyInfo uiMaskProperty = ReflectionCache.GetCachedTypeBoundMetaData<DeclaredUiStringMaskPropertyLookup>(obj.GetType());

            // use ToString when there is no property
            if (uiMaskProperty == null)
            {
                return obj.ToString();
            }

            object uiMaskPropertyValue = uiMaskProperty.PropertyInfo.GetValue(obj);

            if (uiMaskPropertyValue == null)
            {
                return string.Empty;
            }

            var uiMaskPropertyString = uiMaskPropertyValue as string;

            if (uiMaskPropertyString != null)
            {
                return uiMaskPropertyString;
            }

            // if the property value is not a string, search the property type
            return GetUiMaskString(uiMaskPropertyValue);
        }*/
    }
}