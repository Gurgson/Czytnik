using Czytnik_DataAccess.FluentConfig;
using Czytnik_Model.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Czytnik_DataAccess.Database
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Series> Series { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Translator> Translators { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Favourite> Favourites { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<BookDiscount> BooksDiscounts { get; set; }
        public DbSet<BookAuthor> BooksAuthors { get; set; }
        public DbSet<BookTranslator> BooksTranslators { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Keep Identity / other base mappings
            base.OnModelCreating(modelBuilder);

            // Postgres-friendly annotations
            // Limit identifier length to PostgreSQL default (63) and use Postgres identity generation
            modelBuilder.HasAnnotation("Relational:MaxIdentifierLength", 63);

            // Apply existing fluent configurations
            modelBuilder.ApplyConfiguration(new FluentAuthorConfig());
            modelBuilder.ApplyConfiguration(new FluentBookAuthorConfig());
            modelBuilder.ApplyConfiguration(new FluentBookConfig());
            modelBuilder.ApplyConfiguration(new FluentBookDiscountConfig());
            modelBuilder.ApplyConfiguration(new FluentBookTranslatorConfig());
            modelBuilder.ApplyConfiguration(new FluentCartItemConfig());
            modelBuilder.ApplyConfiguration(new FluentCategoryConfig());
            modelBuilder.ApplyConfiguration(new FluentDiscountConfig());
            modelBuilder.ApplyConfiguration(new FluentFavouriteConfig());
            modelBuilder.ApplyConfiguration(new FluentLanguageConfig());
            modelBuilder.ApplyConfiguration(new FluentPublisherConfig());
            modelBuilder.ApplyConfiguration(new FluentReviewConfig());
            modelBuilder.ApplyConfiguration(new FluentSeriesConfig());
            modelBuilder.ApplyConfiguration(new FluentTranslatorConfig());
            modelBuilder.ApplyConfiguration(new FluentUserConfig());
            modelBuilder.ApplyConfiguration(new FluentOrderConfig());
            modelBuilder.ApplyConfiguration(new FluentOrderItemConfig());
        }
    }
}
