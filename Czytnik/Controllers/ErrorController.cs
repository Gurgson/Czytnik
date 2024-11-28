using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Czytnik.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult ErrorPage(int statusCode)
        {
            return View("~/Views/Shared/ErrorPage.cshtml", statusCode);
        }
    }
}
