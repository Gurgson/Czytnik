using Czytnik_Model.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Czytnik_DataAccess.FluentConfig
{
    public class FluentReviewConfig : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> modelBuilder)
        {
            modelBuilder.Property(i => i.Rating).IsRequired();
            modelBuilder.Property(i => i.ReviewText).HasMaxLength(1000);
            modelBuilder.Property(i => i.ReviewDate).IsRequired();

            modelBuilder.HasOne(i => i.Book).WithMany(i => i.Reviews).HasForeignKey(i => i.BookId);
        }
    }
}
