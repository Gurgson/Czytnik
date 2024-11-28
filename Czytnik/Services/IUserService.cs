using Czytnik_Model.Models;
using Czytnik_Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Czytnik.Services
{
    public interface IUserService
    {
       Task<UserProfileViewModel> GetProfileInfo();
       Task<bool> DidUserRateThisBook(int bookId);
        Task<bool> DidUserBefriendThisBook(int bookId);
        Task<List<UserReviewViewModel>> GetUserReviews(int count, string sortOrder);
        Task AddToFavourites(int bookId);
        Task DeleteFavourite(int bookId);
        Task<List<BestBooksViewModel>> GetAllFavourites(int skip = 0, int count = 5, string sortBy = "");
        Task<List<OrderViewModel>> GetOrders(int count, int skip, string sortBy);
        Task<UserSettingsViewModel> GetUserData();
        Task<bool> EditUserData(UserSettingsViewModel userData);
        Task<string> ChangePassword(UserSettingsViewModel userData);
        Task<string> DeleteAccount(UserSettingsViewModel userData);
        Task<string> GetProfilePicture();
    }
}
