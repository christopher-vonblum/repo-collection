using System.Collections.Generic;

namespace CoreUi.Tests.Proxy
{
    public interface IProxyTestModelWithoutSelfReference
    {
        string A { get; set; }
        
        int B { get; set; }
        

        IEnumerable<string> D { get; set; }
        
        IEnumerable<IProxyTestModel> E { get; set; }
    }
}