namespace Czytnik_Model.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public short Quantity { get; set; }

        public Book Book { get; set; }
        public User User { get; set; }
    }
}
