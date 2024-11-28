using Czytnik_Model.Models;
using System;
using System.Collections.Generic;

namespace Czytnik_Model.ViewModels
{
    public class UserReviewViewModel
    {
        public int Id { get; set; }
        public int Rating { get; set; }
        public string ReviewText { get; set; }
        public DateTime ReviewDate { get; set; }
        public string BookTitle { get; set; }
        public List<string> Authors { get; set; }
        public int BookId { get; set; }
    }
}
