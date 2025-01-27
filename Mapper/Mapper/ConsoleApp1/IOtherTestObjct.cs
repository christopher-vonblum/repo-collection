using ConsoleApp1.Repository;

namespace ConsoleApp1
{
    public interface IOtherTestObjct : IEntity<string>
    {
        int Amount { get; set; }
    }
}