using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Czytnik_Model.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Surname { get; set; }
        public DateTime? BirthDate { get; set; }
        public string ProfilePicture { get; set; }

        public ICollection<Review> Reviews { get; set; }
        public ICollection<CartItem> CartItems { get; set; }
        public ICollection<Favourite> Favourites { get; set; }
        public ICollection<Order> Orders { get; set; }


    }
}
