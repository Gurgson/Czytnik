using Microsoft.AspNetCore.Mvc;
public class Footer : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View("Default");
    }
}
