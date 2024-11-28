using Czytnik.Services;
using Czytnik_Model.Models;
using Czytnik_Model.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace Czytnik.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBookService _bookService;

        public HomeController(ILogger<HomeController> logger, IBookService bookService)
        {
            _logger = logger;
            _bookService = bookService;
        }

        public async Task<IActionResult> Index()
        {
            string[] months = new string[] { "Styczniu", "Lutym", "Marcu", "Kwietniu", "Maju", "Czerwcu", "Lipcu", "Sierpniu", "Wrześniu", "Październiku", "Listopadzie", "Grudniu" };
            int month = DateTime.Now.AddMonths(-1).Month;

            dynamic homeView = new ExpandoObject();
            homeView.ofAllTime = await _bookService.GetBestOfAllTimeBooks();
            homeView.month = months[month];

            return View(homeView);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Info()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int code)
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
