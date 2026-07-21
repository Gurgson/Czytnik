using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using Czytnik.Services;
using Czytnik.Settings;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Stripe;
using Stripe.Checkout;

namespace Czytnik.Controllers
{
  public class CheckoutController : Controller
  {
    private readonly ICheckoutService _checkoutService;
    private readonly StripeSettings _stripeSettings;
    private Dictionary<string, decimal> shipments = new Dictionary<string, decimal>();

    public CheckoutController(ICheckoutService checkoutService, IOptions<StripeSettings> stripeSettings)
    {
      _checkoutService = checkoutService;
      _stripeSettings = stripeSettings.Value;

      shipments.Add("point1", 0);
      shipments.Add("point2", 0);
      shipments.Add("shipment1", (decimal)10.95);
      shipments.Add("shipment2", (decimal)12.99);
      shipments.Add("shipment3", (decimal)16.99);
    }
    public IActionResult Index()
    {
      ViewData["StripePublishableKey"] = _stripeSettings.PublishableKey;
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
        PaymentMethodTypes = new List<string> { "card", "blik", "p24" },
      });

      return Json(new { clientSecret = paymentIntent.ClientSecret, key = paymentIntent.Id });
    }

    [HttpPatch]
    public async Task<IActionResult> Update(string shipping, string key, string products, string type, string email)
    {
      products = (products==null) ? "" : products;
      var items = JsonConvert.DeserializeObject<Item[]>(products);
      long Amount = (long)((await CalculateOrderAmount(items, type) + shipments[shipping]) * 100);

      var userId = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

      var options = new PaymentIntentUpdateOptions
      {
        Amount = Amount,
        Metadata = new Dictionary<string, string>
        {
          ["products"] = products,
          ["type"] = type ?? "",
          ["email"] = email ?? "",
          ["userId"] = userId
        }
      };

      var service = new PaymentIntentService();
      service.Update(key, options);

      return Json(new { state = "success" });
    }

    [HttpPost]
    public async Task<IActionResult> Webhook()
    {
      var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

      Event stripeEvent;
      try
      {
        stripeEvent = EventUtility.ConstructEvent(
            json,
            Request.Headers["Stripe-Signature"],
            _stripeSettings.WebhookSecret,
            300,
            throwOnApiVersionMismatch: false);
      }
      catch
      {
        return BadRequest("Invalid Stripe signature");
      }

      if (stripeEvent.Type == "payment_intent.succeeded")
      {
        var pi = stripeEvent.Data.Object as PaymentIntent;
        var service = new PaymentIntentService();

        var current = service.Get(pi.Id);
        if (current.Metadata != null && current.Metadata.ContainsKey("orderId"))
          return Ok();

        var products = GetMeta(current, "products");
        var email = GetMeta(current, "email");
        var userId = GetMeta(current, "userId");

        if (!string.IsNullOrWhiteSpace(products))
        {
          var items = JsonConvert.DeserializeObject<Item[]>(products);
          var paidAmount = pi.Amount / 100m;
          var orderId = await _checkoutService.FulfillOrder(items, email, userId, paidAmount);

          if (orderId != null)
          {
            service.Update(pi.Id, new PaymentIntentUpdateOptions
            {
              Metadata = new Dictionary<string, string> { ["orderId"] = orderId.ToString() }
            });
          }
        }
      }

      return Ok();
    }

    private static string GetMeta(PaymentIntent pi, string key)
        => pi.Metadata != null && pi.Metadata.ContainsKey(key) ? pi.Metadata[key] : "";

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
