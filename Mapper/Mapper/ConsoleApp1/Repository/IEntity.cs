namespace ConsoleApp1.Repository
{
    public interface IEntity<TId>
    {
        TId Id { get; set; }
    }
}