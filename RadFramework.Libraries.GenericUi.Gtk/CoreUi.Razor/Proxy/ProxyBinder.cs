using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using CoreUi.Proxy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Newtonsoft.Json;

namespace CoreUi.Razor.Proxy
{
    public class ProxyBinder :  IModelBinder
    {
        private readonly Type _t;

        public ProxyBinder(Type t)
        {
            _t = t;
        }
        
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            string postBody = null;
            
            using (Stream receiveStream = bindingContext.HttpContext.Request.Body)
            {
                using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8))
                {
                    postBody = readStream.ReadToEnd();
                }
            }
            
            var result = JsonConvert.DeserializeObject(postBody, _t, new ProxyNewtonsoftJsonSerializationConverter());
            
            bindingContext.Result = ModelBindingResult.Success(result);

            return Task.CompletedTask;
        }
    }
}