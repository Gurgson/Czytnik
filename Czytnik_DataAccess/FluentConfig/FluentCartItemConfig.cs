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
    public class FluentCartItemConfig : IEntityTypeConfiguration<CartItem>
    {
        public void Configure(EntityTypeBuilder<CartItem> modelBuilder)
        {
            modelBuilder.Property(i => i.Quantity).IsRequired().HasDefaultValue(1);

            modelBuilder.HasOne(i => i.Book).WithMany(i => i.CartItems).HasForeignKey(i => i.BookId);
        }
    }
}
