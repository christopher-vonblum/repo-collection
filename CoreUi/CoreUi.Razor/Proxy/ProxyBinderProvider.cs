using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CoreUi.Proxy;
using CoreUi.Proxy.Factory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CoreUi.Razor.Proxy
{
    public class ProxyBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (ProxyFactory.IsSimpleField(context.Metadata.ModelType)
             || !context.Metadata.ModelType.IsInterface)
            {
                return null;
            }

            return new ProxyBinder(context.Metadata.ModelType);
        }
    }
}