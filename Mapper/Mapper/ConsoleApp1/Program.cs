using ConsoleApp1.Repository;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            IRepository<string> entityRepo = new MemoryRepository<string>();

            ITestObject o0 = entityRepo.Create<ITestObject>("0");

            o0.Prop1 = "A";

            o0.A = entityRepo.Create<IOtherTestObjct>("1");

            o0.A.Amount = 5;

            var a = o0.A.Amount;
            
            entityRepo.Update(o0);

            o0 = entityRepo.Read<ITestObject>("0");

            string prop = o0.Prop1;

            entityRepo.Delete("0");
        }
    }
}