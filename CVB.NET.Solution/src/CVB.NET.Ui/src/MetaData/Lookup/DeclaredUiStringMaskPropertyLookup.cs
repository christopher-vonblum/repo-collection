namespace CVB.NET.Ui.MetaData.Lookup
{
    /*public class DeclaredUiStringMaskPropertyLookup : CachedSingleMemberLookupBase<CachedPropertyInfo>
    {
        public DeclaredUiStringMaskPropertyLookup(Type reflectedType) : base(reflectedType)
        {
        }

        protected override CachedPropertyInfo Lookup()
        {
            return SelectTypeUiStringMaskProperty(Type);
        }
        private static CachedPropertyInfo SelectTypeUiStringMaskProperty(CachedType type)
        {
            List<CachedPropertyInfo> declaredUiStringMaskProperties =
                type.Properties.Where(
                    prop => prop.Reflected == type
                    && prop.Attributes.OfType<UiStringMaskPropertyAttribute>().Any())
                        .ToList();

            if (declaredUiStringMaskProperties.Count > 1)
            {
                throw new OnlyOneUiStringMaskPropertyAllowedPerTypeException();
            }

            // Search the base type
            if (declaredUiStringMaskProperties.Count == 0 && type != typeof(object))
            {
                // Doing the recursive call using ReflectionCache.GetCachedTypeBoundMetaData can even improve performance here if the property was already cached for the base type
                return ReflectionCache.GetCachedTypeBoundMetaData<DeclaredUiStringMaskPropertyLookup>(type.Reflected.BaseType);
            }

            if (declaredUiStringMaskProperties.Count == 1)
            {
                return declaredUiStringMaskProperties.FirstOrDefault();
            }

            // No property found in complete inheritance chain
            return null;
        }
    }*/
}