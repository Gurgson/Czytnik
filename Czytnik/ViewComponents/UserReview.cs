using Czytnik_Model.Models;
using Czytnik_Model.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

public class UserReview : ViewComponent
{
    public IViewComponentResult Invoke(UserReviewViewModel review, bool isEditable)
    {
        if (isEditable) return View("Editable", review);
        return View("Default",review);
    }
}
