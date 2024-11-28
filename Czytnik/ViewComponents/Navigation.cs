using Czytnik.Services;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;
using System.Threading.Tasks;

public class Navigation : ViewComponent
{
    private readonly ICartService _cartService;
    private readonly IUserService _userService;

    public Navigation(ICartService cartService, IUserService userService)
    {
        _cartService = cartService;
        _userService = userService;
    }
    public async Task<IViewComponentResult> InvokeAsync()
    {
        int? quantity = await _cartService.GetCartQuantity();
        string path = await _userService.GetProfilePicture();
        if(quantity == -1) quantity = null;

        dynamic navView = new ExpandoObject();
        navView.quantity = quantity;
        navView.path = path;
        return View("Default", navView);
    }
}
