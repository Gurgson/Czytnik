using Czytnik_DataAccess.Database;
using Czytnik_Model.Models;
using Czytnik_Model.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Czytnik.Services
{
    public class CartService : ICartService
    {
        private readonly AppDbContext _dbContext;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CartService(AppDbContext dbContext, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<CartItemsViewModel>> GetCartItems()
        {
            var currentUser = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);

            var itemsQuery = _dbContext.CartItems.Where(i => i.User == currentUser).Select(i => new CartItemsViewModel
            {
                bookId = i.BookId,
                userId = i.Id,
                Title = i.Book.Title,
                Price = i.Book.Price,
                Cover = i.Book.Cover,
                Quantity = i.Quantity,
                Authors = i.Book.BookAuthors.Select(ba => $"{ba.Author.FirstName} {ba.Author.SecondName} {ba.Author.Surname}").ToList(),
                Discount = i.Book.BookDiscounts.Where(entry => entry.BookId == i.BookId).Select(entry => entry.Discount).FirstOrDefault(),
            });

            IEnumerable<CartItemsViewModel> result = await itemsQuery.ToListAsync();

            foreach (var item in result)
            {
                item.CalculatedPrice = (item.Discount == null) ? item.Price : CalculateDiscount(item.Price, item.Discount.DiscountValue);
                item.FullPrice = item.CalculatedPrice * item.Quantity;
            }

            if(result.Count() == 0) return null;

            return result;
        }

        public async Task DeleteCartItem(int bookId)
        {
            var currentUser = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            if(currentUser == null) return;

            var cartItem = _dbContext.CartItems.Where(i => i.BookId == bookId && i.User == currentUser).First();
            _dbContext.CartItems.Remove(cartItem);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Clear()
        {
            var currentUser = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            if(currentUser == null) return;

            var cartItemsQuery = _dbContext.CartItems.Where(i => i.User == currentUser);

            _dbContext.CartItems.RemoveRange(cartItemsQuery);
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddCartItem(int bookId)
        {
            var currentUser = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            if(currentUser == null) return;
            if(_dbContext.CartItems.Any(ci => ci.BookId == bookId && ci.User == currentUser)) return;

            var item = new CartItem { BookId = bookId, User = currentUser, Quantity = 1 };

            await _dbContext.CartItems.AddAsync(item);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateQuantity(int bookId, short quantity)
        {
            var currentUser = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            if(currentUser == null) return;

            var item = _dbContext.CartItems.Where(i => i.BookId == bookId && i.User == currentUser).First();
            item.Quantity = quantity;
            await _dbContext.SaveChangesAsync();
        }

        public async Task<int> GetCartQuantity()
        {
            var currentUser = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            if(currentUser == null) return -1;

            var itemsQuery = _dbContext.CartItems.Where(i => i.User == currentUser);
            var items = await itemsQuery.ToListAsync();

            return items.Count;
        }

        public async Task<IEnumerable<CartItemsViewModel>> GetCartBooks(string booksId)
        {
            if(booksId == null) booksId = "";
            string[] list = booksId.Split(',');

            var itemsQuery = _dbContext.Books.Where(b => list.Contains(Convert.ToString(b.Id))).Select(i => new CartItemsViewModel
            {
                bookId = i.Id,
                userId = -1,
                Title = i.Title,
                Price = i.Price,
                Cover = i.Cover,
                Quantity = 1,
                Authors = i.BookAuthors.Select(ba => $"{ba.Author.FirstName} {ba.Author.SecondName} {ba.Author.Surname}").ToList(),
                Discount = i.BookDiscounts.Where(entry => entry.BookId == i.Id).Select(entry => entry.Discount).FirstOrDefault(),
            });


            var items = await itemsQuery.ToListAsync();

            foreach (var item in items)
            {
                item.CalculatedPrice = (item.Discount == null) ? item.Price : CalculateDiscount(item.Price, item.Discount.DiscountValue);
                item.FullPrice = item.CalculatedPrice * item.Quantity;
            }

            return items;
        }

        private decimal CalculateDiscount(decimal price, int discount)
        {
            var percent = 100 - discount;
            var discountPercentage = ((decimal)percent / 100);
            return Math.Round(price * discountPercentage, 2);
        }
    }
}
