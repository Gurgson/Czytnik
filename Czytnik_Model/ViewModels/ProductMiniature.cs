using Czytnik_Model.Models;

namespace Czytnik_Model.ViewModels
{
    public class ProductMiniature
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Cover { get; set; }
        public Category Category { get; set; }
        public decimal Rating { get; set; }
        public decimal Price { get; set; }
    }
}