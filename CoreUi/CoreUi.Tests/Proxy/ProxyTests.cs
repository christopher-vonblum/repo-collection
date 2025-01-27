using System;
using System.Collections.Generic;
using System.Linq;
using CoreUi.Gtk;
using CoreUi.Proxy;
using CoreUi.Proxy.Factory;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NUnit.Framework;

namespace CoreUi.Tests.Proxy
{
    public class ProxyTests
    {
        [SetUp]
        public void Setup()
        {
        }
        
        [Test]
        public void Proxy_Stores_String()
        {
            IProxyTestModel proxy = ProxyFactory.CreateInputModel<IProxyTestModel>();

            string text = "TEST";
            
            proxy.A = text;

            Assert.AreEqual(text, proxy.A);
        }
        
        
        [Test]
        public void Proxy_Stores_Int()
        {
            IProxyTestModel proxy = ProxyFactory.CreateInputModel<IProxyTestModel>();

            int val = 1;
            
            proxy.B = val;

            Assert.AreEqual( val, proxy.B);
        }


        [Test]
        public void Proxy_Converts_Complex_Type_ToProxy()
        {
            IProxyTestModel proxy = ProxyFactory.CreateInputModel<IProxyTestModel>();

            string testValue = "TEST";
            ProxyTestModel model = new ProxyTestModel();
            
            model.A = testValue;
            proxy.C = model;
            
            Assert.IsFalse(proxy is ProxyTestModel);
            Assert.AreEqual( testValue, proxy.C.A);
        }

        [Test]
        public void Proxy_Stores_Proxy_Values_For_Complex_Type()
        {
            IProxyTestModel proxy = ProxyFactory.CreateInputModel<IProxyTestModel>();

            string testValue = "TEST";
            
            IProxyTestModel childProxy = ProxyFactory.CreateInputModel<IProxyTestModel>();
            proxy.C = childProxy;
            
            childProxy.A = testValue;
            
            Assert.AreEqual( testValue, proxy.C.A);
        }
        
        [Test]
        public void Proxy_Converts_IEnumerable_ToProxy()
        {
            IProxyTestModel proxy = ProxyFactory.CreateInputModel<IProxyTestModel>();

            string text = "TEST";
            
            proxy.D = new []{ text };

            Assert.IsTrue(proxy.D.Contains(text));
        }
        
        [Test]
        public void Proxy_Converts_IEnumerable_Of_Complex_Type_ToProxy()
        {
            IProxyTestModel proxy = ProxyFactory.CreateInputModel<IProxyTestModel>();

            string text = "TEST";

            var proxyTestModel = new ProxyTestModel
            {
                A = text
            };
            proxy.E = new []
            {
                proxyTestModel
            };

            Assert.IsTrue(proxy.E.Any(model => model.A == text));
        }
        
        [Test]
        public void Proxy_Can_Be_Json_Serialized()
        {
            IProxyTestModelWithoutSelfReference proxy = ProxyFactory.CreateInputModel<IProxyTestModelWithoutSelfReference>();

            string text = "TEST";

            var proxyTestModel = new ProxyTestModel
            {
                A = text,
                D = new[]{text}
            };
            
            proxy.E = new []
            {
                proxyTestModel
            };

            string json = JsonConvert.SerializeObject(proxy, typeof(IProxyTestModelWithoutSelfReference), 
                new JsonSerializerSettings{Formatting = Formatting.Indented, NullValueHandling = NullValueHandling.Ignore});

            IProxyTestModelWithoutSelfReference deser = JsonConvert.DeserializeObject<IProxyTestModelWithoutSelfReference>(json, new ProxyNewtonsoftJsonSerializationConverter());

            IEnumerable<string> e = deser.E.FirstOrDefault().D;
            
            var x = e.Contains(text);
            
            Assert.IsTrue(deser.E.Any(m => m.A.Equals(text) && m.D.Contains(text)));
        }
    }
}