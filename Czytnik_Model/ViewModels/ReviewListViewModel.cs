using Czytnik_Model.Models;
using System;
using System.Collections.Generic;

namespace Czytnik_Model.ViewModels
{
    public class ReviewListViewModel
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public int ReviewCount { get; set; }
        public decimal? Rating { get; set; }
        public Dictionary<short,int> ReviewsQnt { get; set; }
    }
}
