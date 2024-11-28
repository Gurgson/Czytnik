using Czytnik_Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Czytnik_Model.ViewModels
{
    public class CartItemsViewModel
    {
        public int bookId { get; set; }
        public int userId { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public decimal FullPrice { get; set; }
        public string Cover { get; set; }
        public int Quantity { get; set; }
        public decimal CalculatedPrice { get; set; }
        public List<string> Authors { get; set; }
        public Discount Discount { get; set; }
    }
}
