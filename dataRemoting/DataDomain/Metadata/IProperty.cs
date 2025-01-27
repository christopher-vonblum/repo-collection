namespace DataDomain
{
    public interface IProperty
    {
        bool IsIndexed { get; set; }
        bool IsRuntimeReference { get; set; }
        bool IsEnumerable { get; set; }
        IClrType PropertyType { get; set; }
        IClrType DeclaringType { get; set; }
        string Name { get; set; }
    }
}