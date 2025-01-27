namespace CVB.NET.Exceptions.Reflection
{
    using System;
    using System.Collections.Generic;

    public class NoCompatibleConstructorFoundException : Exception
    {
        public NoCompatibleConstructorFoundException(Type type, IEnumerable<string> argumentNames) : base(GetMessage(type, argumentNames))
        {
        }

        private static string GetMessage(Type type, IEnumerable<string> argumentNames)
        {
            return String.Format("No compatible constructor found for type {0} matching arguments ({1}).",
                type.FullName, string.Join(", ", argumentNames));
        }
    }
}