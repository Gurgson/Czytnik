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
    public class FluentBookDiscountConfig : IEntityTypeConfiguration<BookDiscount>
    {
        public void Configure(EntityTypeBuilder<BookDiscount> modelBuilder)
        {
            modelBuilder.HasKey(i => new { i.BookId, i.DiscountId });
            modelBuilder.HasOne(i => i.Book).WithMany(i => i.BookDiscounts).HasForeignKey(i => i.BookId);
            modelBuilder.HasOne(i => i.Discount).WithMany(i => i.BookDiscounts).HasForeignKey(i => i.DiscountId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
