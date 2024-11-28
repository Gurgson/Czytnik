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
using static Czytnik.Controllers.CheckoutController;

namespace Czytnik.Services
{
    public class CheckoutService : ICheckoutService
    {
        private readonly AppDbContext _dbContext;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CheckoutService(AppDbContext dbContext, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task AddOrder(Item[] items, string type)
        {
            if(type == null) return;
            var currentUser = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);

            var order = new Order { OrderDate = DateTime.Now, User = currentUser };

            await _dbContext.Orders.AddAsync(order);
            await _dbContext.SaveChangesAsync();

            int booksCounter = 0;

            int orderId = order.Id;

            if (currentUser == null || type == "single") {
                foreach (Item item in items) {
                    OrderItem newOrderItem = new OrderItem {
                        OrderId = orderId,
                        BookId = Convert.ToInt32(item.Id),
                        Quantity = item.Quantity,
                        Price = _dbContext.Books.Where(b => b.Id == Convert.ToInt32(item.Id)).Select(b => b.Price).FirstOrDefault()
                    };

                    await _dbContext.OrderItems.AddAsync(newOrderItem);
                    await _dbContext.SaveChangesAsync();
                    await UpdateNumberOfCopiesSold(newOrderItem.BookId,newOrderItem.Quantity);
                }
            } else {
                var itemsQuery = _dbContext.CartItems.Where(i => i.User == currentUser).Select(i => new OrderItem
                {
                    BookId = i.BookId,
                    OrderId = orderId,
                    Quantity = i.Quantity,
                    Price = _dbContext.Books.Where(b => b.Id == i.BookId).Select(b => b.Price).FirstOrDefault()
                });

                IEnumerable<OrderItem> orderItems = await itemsQuery.ToListAsync();

                booksCounter = orderItems.Count();

                foreach (OrderItem newOrderItem in orderItems) {
                    await _dbContext.OrderItems.AddAsync(newOrderItem);
                    await _dbContext.SaveChangesAsync();
                    await UpdateNumberOfCopiesSold(newOrderItem.BookId, newOrderItem.Quantity);
                }
            }

            if(type == "single"){
              if(items.Length == 0){
                var orderItem = _dbContext.Orders.Where(i => i.Id == orderId).First();
                _dbContext.Orders.Remove(orderItem);
                await _dbContext.SaveChangesAsync();
              }
            }else if(type == "cart"){
              if(currentUser != null && booksCounter == 0){
                var orderItem = _dbContext.Orders.Where(i => i.Id == orderId).First();
                _dbContext.Orders.Remove(orderItem);
                await _dbContext.SaveChangesAsync();
              }
            }
        }

        public async Task UpdateNumberOfCopiesSold(int bookId, int quantity = 1)
        {
            var book = _dbContext.Books.Where(b => b.Id == bookId).FirstOrDefault();
            if(book != null)
            {
                book.NumberOfCopiesSold += (short)quantity;
                await _dbContext.SaveChangesAsync();
            }
        }

    public async Task<decimal> CalculatePrice(Item[] items, string type)
    {
      var currentUser = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
      if (currentUser == null || type == "single")
      {
        if(items == null) return 1400;
        List<string> ids = items.Select(item => item.Id).ToList();

        var booksQuery = _dbContext.Books.Where(b => ids.Contains(Convert.ToString(b.Id))).Select(i => new CartItemsViewModel
        {
          bookId = i.Id,
          userId = -1,
          Title = i.Title,
          Price = i.Price,
          Cover = i.Cover,
          Quantity = -1,
          Authors = i.BookAuthors.Select(ba => $"{ba.Author.FirstName} {ba.Author.SecondName} {ba.Author.Surname}").ToList(),
          Discount = i.BookDiscounts.Where(entry => entry.BookId == i.Id).Select(entry => entry.Discount).FirstOrDefault(),
        });

        var books = await booksQuery.ToListAsync();
        decimal sumPrice = 0;

        foreach (var book in books)
        {
          book.Quantity = Array.Find(items, item => item.Id == Convert.ToString(book.bookId)).Quantity;
          book.CalculatedPrice = (book.Discount == null) ? book.Price : CalculateDiscount(book.Price, book.Discount.DiscountValue);
          sumPrice += book.CalculatedPrice * book.Quantity;
        }

        return sumPrice;
      }
      else
      {
        var booksQuery = _dbContext.CartItems.Where(i => i.User == currentUser).Select(i => new CartItemsViewModel
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

        IEnumerable<CartItemsViewModel> books = await booksQuery.ToListAsync();

        decimal sumPrice = 0;
        foreach (var book in books)
        {
          book.CalculatedPrice = (book.Discount == null) ? book.Price : CalculateDiscount(book.Price, book.Discount.DiscountValue);
          book.FullPrice = book.CalculatedPrice * book.Quantity;
          sumPrice += book.CalculatedPrice * book.Quantity;
        }

        return sumPrice;
      }
    }

    private decimal CalculateDiscount(decimal price, int discount)
    {
            var percent = 100 - discount;
            var discountPercentage = ((decimal)percent / 100);
            return Math.Round(price * discountPercentage, 2);
        }
  }
}
