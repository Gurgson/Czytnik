using Czytnik_DataAccess.Database;
using Czytnik_Model.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Czytnik
{
    public class Program
    {
        public static void Main(string[] args)
        {
            /// Kod do wrzucania danych
            /*
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer("Data Source=czytnikdbserver.database.windows.net;Initial Catalog=Czytnik_db;User ID=baqardo;Password=zaq1@WSX;Connect Timeout=30;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

            #region CustomSeeding
            using (AppDbContext context = new AppDbContext(optionsBuilder.Options))
            using (var transaction = context.Database.BeginTransaction())
            {
                //context.Database.EnsureCreated();
                //context.Database.OpenConnection();
                try
                {
                    //Dodaj tutaj

                    { }

                    //Tutaj skończ
                    //context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Books ON;");
                    context.SaveChanges();
                    //context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Books OFF;");
                    transaction.Commit();
                }
                finally
                {
                    context.Database.CloseConnection();
                }
            }
            #endregion*/
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}