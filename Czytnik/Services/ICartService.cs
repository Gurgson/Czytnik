using Czytnik_Model.Models;
using Czytnik_Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Czytnik.Services
{
    public interface ICartService
    {
        Task<IEnumerable<CartItemsViewModel>> GetCartItems();
        Task DeleteCartItem(int bookId);
        Task AddCartItem(int bookId);
        Task UpdateQuantity(int bookId, short quantity);
        Task<int> GetCartQuantity();
        Task<IEnumerable<CartItemsViewModel>> GetCartBooks(string booksId);
        Task Clear();
    }
}
