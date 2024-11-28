using Czytnik_Model.Models;
using System;

namespace Czytnik_Model.ViewModels
{
    public class ReviewViewModel
    {
        public int Id { get; set; }
        public int Rating { get; set; }
        public string Username { get; set; }
        public string ReviewText { get; set; }
        public DateTime ReviewDate { get; set; }
    }
}
