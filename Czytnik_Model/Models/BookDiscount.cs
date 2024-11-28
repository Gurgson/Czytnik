using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Czytnik_Model.Models
{
    public class BookDiscount
    {
        public int BookId { get; set; }
        public int DiscountId { get; set; }

        public Book Book { get; set; }
        public Discount Discount { get; set; }
    }
}
