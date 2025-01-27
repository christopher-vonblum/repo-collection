using ConsoleApp1.Repository;

namespace ConsoleApp1
{
    public interface ITestObject : IEntity<string>
    {
        string Prop1 { get; set; }
        
        string Teest { get; set; }
        IOtherTestObjct A { get; set; }
        
    }
}