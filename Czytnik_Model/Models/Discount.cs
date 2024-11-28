using System.Collections.Generic;

namespace Czytnik_Model.Models
{
    public class Discount
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public short DiscountValue { get; set; }

        public ICollection<BookDiscount> BookDiscounts { get; set; }

    }
}
