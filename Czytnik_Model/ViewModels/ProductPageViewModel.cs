using Czytnik_Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Czytnik_Model.ViewModels
{
    public class ProductPageViewModel
    {
        public Book Product { get; set; }
        public decimal CalculatedPrice { get; set; }
        public List<string> Authors { get; set; }
        public Discount Discount { get; set; }
        public int Bestseller { get; set; }
        public string Publisher { get; set; }
        public Category Category { get; set; }
        public int ReviewCount { get; set; }
        public List<ReviewViewModel> Reviews { get; set; }
        public List<string> Translators { get; set; }
        public string OriginalLanguage { get; set; }
        public string EditionLanguage { get; set; }
        public Series Series { get; set; }
        public bool IsFormVisible { get; set; }
        public bool IsLikedByUser { get; set; }
    }
}
