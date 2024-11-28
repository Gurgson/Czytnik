using System;

namespace Czytnik_Model.Models
{
    public class Review
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public short Rating { get; set; }
        public string ReviewText { get; set; }
        public DateTime ReviewDate { get; set; }

        public Book Book { get; set; }
        public User User { get; set; }

    }
}
