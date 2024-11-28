using Czytnik.Services;
using Czytnik_Model.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace Czytnik.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookService _bookService;
        private readonly IReviewService _reviewService;
        private readonly IUserService _userService;

        public BookController(IBookService bookService, IReviewService reviewService, IUserService userService)
        {
            _bookService = bookService;
            _reviewService = reviewService;
            _userService = userService;
        }
        public IActionResult Index(int Id)
        {
            var books = _bookService.GetProductBookPage(Id);
            books.IsFormVisible = _userService.DidUserRateThisBook(Id).Result;
            books.IsLikedByUser = _userService.DidUserBefriendThisBook(Id).Result;
            return View(books);
        }

        public IActionResult ReviewsList(int Id)
        {
            var reviewView = _reviewService.GetReviewList(Id);
            return View(reviewView);
        }
    }
}
