using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreUi.Web.Controllers
{
    public abstract class ServiceProxyControllerBase : Controller
    {
        [HttpPost]
        public IActionResult GetMethods()
        {
            return Json(this.GetType()
                .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy)
                .Where(m => m.GetCustomAttributes(true).OfType<HttpPostAttribute>().Any())
                .Select(m => m.Name));
        }
    }
}