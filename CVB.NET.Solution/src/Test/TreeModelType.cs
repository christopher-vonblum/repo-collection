using CVB.NET.DataAccess.Sql.TreeModelRepository.Schema;

namespace Test
{
    public class TreeModelType : TreeModelBase
    {
        public string Test { get; set; }

        public TestModelType2 TestModel2 { get; set; }
    }
}