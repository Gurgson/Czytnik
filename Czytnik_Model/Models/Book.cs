using System;
using System.Collections.Generic;

namespace Czytnik_Model.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public string Cover { get; set; }
        public int? OriginalLanguageId { get; set; }
        public int? EditionLanguageId { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string IssueNumber { get; set; }
        public short PageQuantity { get; set; }
        public string Dimensions { get; set; }
        public int CategoryId { get; set; }
        public int? SeriesId { get; set; }
        public int PublisherId { get; set; }
        public decimal? Rating { get; set; }
        public bool? IsInStock { get; set; }
        public short NumberOfCopiesSold { get; set; }

        public Language OriginalLanguage { get; set; }
        public Language EditionLanguage { get; set; }
        public Category Category { get; set; }
        public Series Series { get; set; }
        public Publisher Publisher { get; set; }
        public ICollection<BookDiscount> BookDiscounts { get; set; }
        public ICollection<BookAuthor> BookAuthors { get; set; }
        public ICollection<BookTranslator> BookTranslators { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public ICollection<CartItem> CartItems { get; set; }
        public ICollection<Favourite> Favourites { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
