namespace CVB.NET.TextTemplating.Runtime.Base
{
    using System;
    using System.Linq;
    using PostSharp.Patterns.Contracts;
    using Reflection.Caching.Cached;

    /// <summary>
    /// Utility class to produce culture-oriented representation of an object as a string.
    /// </summary>
    public class ToStringInstanceHelper
    {
        /// <summary>
        /// Gets or sets format provider to be used by ToStringWithCulture method.
        /// </summary>
        [NotNull]
        public IFormatProvider FormatProvider { get; set; } = System.Globalization.CultureInfo.InvariantCulture;

        /// <summary>
        /// This is called from the compile/run appdomain to convert objects within an expression block to a string
        /// </summary>
        public string ToStringWithCulture(object objectToConvert)
        {
            if (objectToConvert == null)
            {
                return string.Empty;
            }

            CachedType objectType = objectToConvert.GetType();

            CachedMethodInfo toStringMethod = objectType
                .Methods
                .FirstOrDefault(method
                    => "ToString".Equals(method.InnerReflectionInfo.Name)
                       && method.CachedParameterInfos.Length == 1
                       && method.CachedParameterInfos.SingleOrDefault(
                           param => param.InnerReflectionInfo.ParameterType == typeof (IFormatProvider)) != null);

            if (toStringMethod == null)
            {
                return objectToConvert.ToString();
            }

            return (string) toStringMethod.InnerReflectionInfo.Invoke(objectToConvert, new object[] {FormatProvider});
        }
    }
}