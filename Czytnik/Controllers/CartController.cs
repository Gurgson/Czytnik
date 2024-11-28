using Czytnik.Services;
using Czytnik_Model.Models;
using Czytnik_Model.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace Czytnik.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartController(ICartService cartService, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _cartService = cartService;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);

            var cartItems = await _cartService.GetCartItems();
            if(currentUser != null && cartItems == null) return View("Empty");
            return View(cartItems);
        }
        public IActionResult Empty()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetBooks(string booksId)
        {
            IEnumerable<CartItemsViewModel> books = await _cartService.GetCartBooks(booksId);
            var json = JsonSerializer.Serialize(books);

            return Json(json);
        }
        [HttpDelete]
        public async Task<JsonResult> DeleteItem(int bookId)
        {
            await _cartService.DeleteCartItem(bookId);
            return Json(null);
        }

        [HttpPost]
        public async Task<IActionResult> AddItem(int bookId)
        {
            await _cartService.AddCartItem(bookId);
            return Ok("{}");
        }
        [HttpDelete]
        public async Task<IActionResult> Clear()
        {
            await _cartService.Clear();
            return Ok("{}");
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateQuantity(int bookId, short quantity)
        {
            await _cartService.UpdateQuantity(bookId, quantity);
            return Ok();
        }
    }
}
