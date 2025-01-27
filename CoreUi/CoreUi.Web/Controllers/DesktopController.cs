using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CoreUi.Web.Models;
using Microsoft.AspNetCore.Authorization;

namespace CoreUi.Web.Controllers
{
    [Authorize]
    public class DesktopController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
                
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}