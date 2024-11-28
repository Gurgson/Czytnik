using Microsoft.AspNetCore.Mvc;
public class ReviewForm : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View("Default");
    }
}
