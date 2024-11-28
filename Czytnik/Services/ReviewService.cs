using Czytnik_DataAccess.Database;
using Czytnik_Model.Models;
using Czytnik_Model.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Czytnik.Services
{
    public class ReviewService : IReviewService
    {
        private readonly AppDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager;
        public ReviewService(AppDbContext dbContext, IHttpContextAccessor accessor, UserManager<User> userManager)
        {
            _dbContext = dbContext;
            _httpContextAccessor = accessor;
            _userManager = userManager;
        }

        public async Task<IEnumerable<ReviewViewModel>> GetAll(int Id, int skip=0, int count=10)
        {
            var reviews = _dbContext.Reviews
                .Where(r => r.BookId == Id)
                .Select(r => new ReviewViewModel
                {
                    Id = r.Id,
                    Rating = r.Rating,
                    Username = r.User.UserName,
                    ReviewText = r.ReviewText,
                    ReviewDate = r.ReviewDate,
                })
                .OrderByDescending(r => r.ReviewDate);
            var result = await reviews.Skip(skip).Take(count).ToListAsync();
            return result;
        }

        public async Task<IEnumerable<UserReviewViewModel>> GetAllUser(int skip = 0, int count = 5, string sortBy = "")
        {
            var currentUser = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            var reviews = _dbContext.Reviews
                .Where(r => r.User == currentUser)
                .Select(r => new UserReviewViewModel
                {
                    Id = r.Id,
                    Rating = r.Rating,
                    ReviewText = r.ReviewText,
                    ReviewDate = r.ReviewDate,
                    BookTitle = r.Book.Title,
                    Authors = r.Book.BookAuthors.Select(ba => $"{ba.Author.FirstName} {ba.Author.SecondName} {ba.Author.Surname}").ToList(),
                    BookId = r.BookId
                });


            switch (sortBy)
            {
                case "rating-desc":
                    reviews = reviews.OrderByDescending(r => r.Rating);
                    break;
                case "rating-asc":
                    reviews = reviews.OrderBy(r => r.Rating);
                    break;
                case "date-desc":
                    reviews = reviews.OrderByDescending(r => r.ReviewDate);
                    break;
                default:
                    reviews = reviews.OrderBy(r => r.ReviewDate);
                    break;
            }

            var result = await reviews.Skip(skip).Take(count).ToListAsync();
            return result;
        }

        public ReviewListViewModel GetReviewList(int BookId)
        {
            var reviewList = _dbContext.Books
                .Where(b => b.Id == BookId)
                .Select(b => new ReviewListViewModel
                {
                    BookId = BookId,
                    Title = b.Title,
                    ReviewCount = b.Reviews.Count,
                    Rating = b.Rating
                }).SingleOrDefault();

            var reviewCount = _dbContext.Reviews
                .Where(el => el.BookId == BookId)
                .GroupBy(r => r.Rating)
                .Select(g => new { rating = g.Key, count = g.Count() }).ToList();

            reviewList.ReviewsQnt = reviewCount.ToDictionary(x=>x.rating,x=>x.count);

            return reviewList ;
        }

        public async Task Add(Review review)
        {
            var currentUser = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            if(currentUser != null)
            {
                var entity = new Review
                {
                    Rating = review.Rating,
                    ReviewDate = DateTime.Now,
                    BookId = review.BookId,
                    ReviewText = review.ReviewText,
                    User = currentUser
                };
                await _dbContext.Reviews.AddAsync(entity);
                await _dbContext.SaveChangesAsync();
                CalculateAverageRating(review.BookId);
            }
        }

        public async Task Delete(int Id)
        {
            var currentUser = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            var review = _dbContext.Reviews.Where(r => r.Id == Id).FirstOrDefault();
            int bookId = review.BookId;
            if(review.User == currentUser)
            {
                _dbContext.Reviews.Remove(review);
                await _dbContext.SaveChangesAsync();
                CalculateAverageRating(bookId);
            }
        }

        public async Task Edit(Review review, int Id)
        {
            var currentUser = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            if (currentUser != null)
            {
                var _review = _dbContext.Reviews.Where(r => r.Id == Id).FirstOrDefault();
                
                _review.Rating = review.Rating;
                _review.ReviewText = review.ReviewText;
                
                await _dbContext.SaveChangesAsync();
                CalculateAverageRating(_review.BookId);
            }
        }

        public void CalculateAverageRating(int bookId)
        {
            var book = _dbContext.Books.SingleOrDefault(b => b.Id == bookId);
            var reviews = _dbContext.Books.Where(b => b.Id == bookId).Select(b => b.Reviews).FirstOrDefault();
            double reviewSum = reviews.Sum(r => r.Rating);
            decimal avgRating = (decimal)reviewSum / reviews.Count;
            decimal avgRatingFixed = Math.Round(avgRating, 2);

            book.Rating = avgRatingFixed;
            _dbContext.SaveChanges();
        }
    }
}
