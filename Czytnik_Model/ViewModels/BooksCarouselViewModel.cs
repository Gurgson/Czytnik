using Czytnik_Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Czytnik_Model.ViewModels
{
    public class BooksCarouselViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string Cover { get; set; }
        public decimal? Rating { get; set; }
        public Category Category { get; set; }
        public List<string> Authors { get; set; }   
        public Discount Discount { get; set; }
    }
}
