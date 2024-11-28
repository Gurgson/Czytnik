using Czytnik_Model.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace Czytnik_Model.ViewModels
{
    public class UserSettingsViewModel
    {
        public string Username { get; set; }
        public string? FirstName { get; set; }
        public string? SecondName { get; set; }
        public string? Surname { get; set; }
        public DateTime? Birthdate { get; set; }
        public string? PhoneNumber { get; set; }
        public string ProfilePath { get; set; }
        public IFormFile ProfilePicture { get; set; }

        public string CurrentPassword { get; set; }
        public string RepeatCurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string RepeatNewPassword { get; set; }
    }
}
