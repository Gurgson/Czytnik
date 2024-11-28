using Czytnik_Model.Models;
using Czytnik_Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Czytnik.Services
{
    public interface IReviewService
    {
        Task<IEnumerable<ReviewViewModel>> GetAll(int Id, int skip = 0, int count = 3);
        ReviewListViewModel GetReviewList(int BookId);
        Task Add(Review review);
        Task Delete(int Id);
        Task Edit(Review review, int Id);
        void CalculateAverageRating(int bookId);
        Task<IEnumerable<UserReviewViewModel>> GetAllUser(int skip = 0, int count = 5, string sortBy = "");
    }
}
