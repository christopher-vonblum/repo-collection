namespace CVB.NET.Reflection.Caching.Aspect
{
    using System;
    using System.Reflection;
    using PostSharp.Patterns.Contracts;

    public class UseLookupAttribute : Attribute
    {
        public Delegate LookupMethod { get; }

        public UseLookupAttribute([NotNull] Type lookupType, [NotEmpty] string method)
        {
            PropertyInfo property = lookupType.GetProperty(method, BindingFlags.Public | BindingFlags.Static);

            FieldInfo field = null;

            if (property == null)
            {
                field = lookupType.GetField(method, BindingFlags.Public | BindingFlags.Static);
            }

            LookupMethod = (Delegate) (property == null ? field.GetValue(null) : property.GetValue(null));
        }
    }
}