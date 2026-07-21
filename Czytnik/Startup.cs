using Czytnik.Services;
using Czytnik.Settings;
using Czytnik_DataAccess.Database;
using Czytnik_Model.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Czytnik
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
      services.Configure<StripeSettings>(Configuration.GetSection("Stripe"));

      var stripeSecretKey = Configuration["Stripe:SecretKey"] ?? "";
      var stripePublishableKey = Configuration["Stripe:PublishableKey"] ?? "";
      var allowLiveMode = Configuration.GetValue<bool>("Stripe:AllowLiveMode");
      if (!allowLiveMode &&
          (stripeSecretKey.StartsWith("sk_live_") || stripePublishableKey.StartsWith("pk_live_")))
      {
        throw new InvalidOperationException(
            "Wykryto klucz Stripe LIVE, a aplikacja jest zablokowana w trybie sandbox. " +
            "Użyj kluczy testowych (sk_test_ / pk_test_). Aby świadomie włączyć realne płatności " +
            "ustaw Stripe:AllowLiveMode=true.");
      }

      Stripe.StripeConfiguration.ApiKey = stripeSecretKey;

      services.Configure<EmailSettings>(Configuration.GetSection("Email"));
      services.AddTransient<IEmailService, EmailService>();

      services.AddDefaultIdentity<User>().AddEntityFrameworkStores<AppDbContext>();
      services.AddRazorPages().AddRazorRuntimeCompilation();
      services.AddDbContext<AppDbContext>(
          config => config.UseNpgsql(Configuration.GetConnectionString("Application"))
      );
      services.AddControllersWithViews();

      services.AddTransient<ICategoryService, CategoryService>();
      services.AddTransient<IUserService, UserService>();
      services.AddTransient<IBookService, BookService>();
      services.AddTransient<ILanguageService, LanguageService>();
      services.AddTransient<IReviewService, ReviewService>();
      services.AddTransient<ICartService, CartService>();
      services.AddTransient<ICheckoutService, CheckoutService>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      var forwardedOptions = new ForwardedHeadersOptions
      {
        ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
      };
      forwardedOptions.KnownNetworks.Clear();
      forwardedOptions.KnownProxies.Clear();
      app.UseForwardedHeaders(forwardedOptions);

      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      else
      {
        app.UseExceptionHandler("/Home/Error"); //jezeli akcja zwr�ci b��d
                                                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
      }
      app.UseStatusCodePagesWithRedirects("/Error/ErrorPage?statusCode={0}"); // jezeli nie ma akcji
      app.UseHttpsRedirection();
      app.UseStaticFiles();

      app.UseRouting();

      app.UseAuthentication();
      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllerRoute(
                  name: "default",
                  pattern: "{controller=Home}/{action=Index}/{id?}");
        endpoints.MapRazorPages();
      });
    }
  }
}
