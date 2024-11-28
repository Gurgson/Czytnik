using Czytnik.Services;
using Czytnik_Model.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Dynamic;
using System.Threading.Tasks;

namespace Czytnik.Controllers
{
    public class SearchController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly ILanguageService _languageService;
        private readonly IBookService _bookService;
        public SearchController(ICategoryService categoryService, ILanguageService languageService, IBookService bookService)
        {
            _categoryService = categoryService;
            _languageService = languageService;
            _bookService = bookService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(Search search)
        {
            dynamic searchView = new ExpandoObject();
            var booksData = await _bookService.SearchBooks(search);
            searchView.books = booksData.Item1;
            searchView.booksCount = booksData.Item2;
            searchView.search = search;

            return View(searchView);
        }

        [HttpGet]
        public async Task<JsonResult> GetAllFilters()
        {
            var categoriesViewModel = await _categoryService.GetAll();
            var languagesViewModel = await _languageService.GetAll();

            var filtersResult = new { categories = categoriesViewModel, languages = languagesViewModel };

            return Json(filtersResult);
        }
    }
}
