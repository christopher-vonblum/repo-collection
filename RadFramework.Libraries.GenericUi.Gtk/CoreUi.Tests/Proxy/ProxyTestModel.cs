using System.Collections.Generic;

namespace CoreUi.Tests.Proxy
{
    class ProxyTestModel : IProxyTestModel
    {
        public string A { get; set; }
        public int B { get; set; }
        public IProxyTestModel C { get; set; }
        public IEnumerable<string> D { get; set; }
        public IEnumerable<IProxyTestModel> E { get; set; }
    }
}