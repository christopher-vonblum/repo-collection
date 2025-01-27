namespace CVB.NET.Abstractions.Ioc.Utils
{
    public static class HashcodeExtensions
    {
        public static int GetNamedHashcode(this object obj, string name)
        {
            return name.GetHashCode() + obj.GetType().AssemblyQualifiedName.GetHashCode() + obj.GetHashCode();
        }
    }
}