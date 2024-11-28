using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Czytnik.Services;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Stripe;
using Stripe.Checkout;

namespace Czytnik.Controllers
{
  public class CheckoutController : Controller
  {
    private readonly ICheckoutService _checkoutService;
    private Dictionary<string, decimal> shipments = new Dictionary<string, decimal>();

    public CheckoutController(ICheckoutService checkoutService)
    {
      _checkoutService = checkoutService;

      shipments.Add("point1", 0);
      shipments.Add("point2", 0);
      shipments.Add("shipment1", (decimal)10.95);
      shipments.Add("shipment2", (decimal)12.99);
      shipments.Add("shipment3", (decimal)16.99);
    }
    public IActionResult Index()
    {
      StripeConfiguration.ApiKey = "sk_test_51KPASKGvoTqJgR6RzAxqrWRehxLko3wYbsphwxrzUmi8tgRsRxtOLm20B9LWgHXBfFLqM2uIBxKnwwqJI8amTD1t00Ww8W9c02";
      return View();
    }

    public IActionResult Success()
    {
      return View();
    }

    [HttpPost]
    public IActionResult Session(string products)
    {
      var paymentIntentService = new PaymentIntentService();

      var paymentIntent = paymentIntentService.Create(new PaymentIntentCreateOptions
      {
        Amount = 1400,
        Currency = "pln",
        AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
        {
          Enabled = true,
        },
      });

      return Json(new { clientSecret = paymentIntent.ClientSecret, key = paymentIntent.Id });
    }

    [HttpPost]
    public async Task Order(string products, string type)
    {
      products = (products == null) ? "" : products;
      var items = JsonConvert.DeserializeObject<Item[]>(products);

      await _checkoutService.AddOrder(items, type);
    }

    [HttpPatch]
    public async Task<IActionResult> Update(string shipping, string key, string products, string type)
    {
      products = (products==null) ? "" : products;
      var items = JsonConvert.DeserializeObject<Item[]>(products);
      long Amount = (long)((await CalculateOrderAmount(items, type) + shipments[shipping]) * 100);
      var options = new PaymentIntentUpdateOptions
      {
        Amount = Amount
      };

      var service = new PaymentIntentService();
      service.Update(key, options);

      return Json(new { state = "success" });
    }

    private async Task<decimal> CalculateOrderAmount(Item[] items, string type)
    {
      decimal price = await _checkoutService.CalculatePrice(items, type);

      return price;
    }

    public class Item
    {
      [JsonProperty("bookId")]
      public string Id { get; set; }
      [JsonProperty("quantity")]
      public int Quantity { get; set; }
    }
  }
}
