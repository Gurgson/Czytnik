using Czytnik.Services;
using Czytnik_Model.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Czytnik.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly IReviewService _reviewService;
        public ReviewsController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<JsonResult> GetAllReviews(int Id, int skip=0, int count=10)
        {
            var reviewsViewModel = await _reviewService.GetAll(Id,skip,count);
            return Json(reviewsViewModel);
        }
        [HttpGet]
        public async Task<JsonResult> GetAllUserReviews(int skip = 0, int count = 5, string sortBy="")
        {
            var reviewsViewModel = await _reviewService.GetAllUser(skip, count, sortBy);
            return Json(reviewsViewModel, new JsonSerializerOptions { PropertyNamingPolicy = null });
        }

        [HttpPost]
        public async Task<IActionResult> Add(Review review)
        {
            await _reviewService.Add(review);
            return Ok("{}");
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int Id)
        {
            await _reviewService.Delete(Id);
            return Ok("{}");
        }

        [HttpPatch]
        public async Task<IActionResult> Edit(Review review, int Id)
        {
            await _reviewService.Edit(review, Id);
            return Ok("{}");
        }
    }
}
