namespace CVB.NET.Utils.Array
{
    public static class ArrayExtensions
    {
        public static TTarget[] CastArray<TTarget>(this object[] source)
        {
            return System.Array.ConvertAll(source, i => (TTarget) i);
        }
    }
}