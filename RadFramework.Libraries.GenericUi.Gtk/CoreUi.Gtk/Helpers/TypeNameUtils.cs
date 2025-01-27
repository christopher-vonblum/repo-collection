using System;
using System.Linq;

namespace CoreUi.Gtk.Helpers
{
    public static class TypeNameUtils
    {
        public static string FriendlyTypeName(Type t)
        {
            string name = t.Name;

            Type[] genericArgs = t.GetGenericArguments();

            if (genericArgs.Any())
            {
                name += $"<{string.Join(", ", genericArgs.Select(a => FriendlyTypeName(a)))}>";
            }

            return name;
        }
    }
}