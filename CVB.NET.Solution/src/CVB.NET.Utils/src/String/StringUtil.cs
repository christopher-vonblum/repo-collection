namespace CVB.NET.Utils.String
{
    using PostSharp.Patterns.Contracts;

    public static class StringUtil
    {
        public static string EnsureSinglePrefix([NotNull] this string value, [NotEmpty] string prefix)
        {
            return value.EnsureNoPrefixes(prefix).EnsurePrefix(prefix);
        }

        public static string EnsurePrefix([NotNull] this string value, [NotEmpty] string prefix)
        {
            if (!value.StartsWith(prefix))
            {
                return prefix + value;
            }

            return value;
        }

        public static string EnsureSingleSuffix([NotNull] this string value, [NotEmpty] string suffix)
        {
            return value.EnsureNoSuffixes(suffix).EnsureSuffix(suffix);
        }

        public static string EnsureSuffix([NotNull] this string value, [NotEmpty] string suffix)
        {
            if (!value.EndsWith(suffix))
            {
                return value + suffix;
            }

            return value;
        }

        public static string EnsureNoPrefix([NotNull] this string value, [NotEmpty] string prefix)
        {
            if (value.StartsWith(prefix))
            {
                return value.Substring(prefix.Length);
            }

            return value;
        }

        public static string EnsureNoPrefixes([NotNull] this string value, [NotEmpty] string prefix)
        {
            string removePrefixes = value;

            while (removePrefixes.StartsWith(prefix))
            {
                removePrefixes = removePrefixes.EnsureNoPrefix(prefix);
            }

            return removePrefixes;
        }

        public static string EnsureNoSuffix([NotNull] this string value, [NotEmpty] string suffix)
        {
            if (value.EndsWith(suffix))
            {
                return value.Substring(0, value.Length - suffix.Length);
            }

            return value;
        }

        public static string EnsureNoSuffixes([NotNull] this string value, [NotEmpty] string suffix)
        {
            string removeSuffixes = value;

            while (removeSuffixes.EndsWith(suffix))
            {
                removeSuffixes = removeSuffixes.EnsureNoSuffix(suffix);
            }

            return removeSuffixes;
        }
    }
}