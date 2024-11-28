using Czytnik_Model.Models;
using System;
using System.Collections.Generic;

namespace Czytnik_Model.ViewModels
{
    public class UserProfileViewModel
    {
        public string Username { get; set; }
        public string? FirstName { get; set; }
        public string? SecondName { get; set; }
        public string? Surname { get; set; }
        public DateTime? Birthdate { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string ProfilePicture { get; set; }
        public string FullName
        {
            get { return string.Format("{0} {1}", FirstName, Surname); }
        }
        public List<UserReviewViewModel> UserReviews { get; set; }
        public List<BestBooksViewModel> Favourites { get; set; }
        public List<OrderViewModel> Orders { get; set; }
    }
}
