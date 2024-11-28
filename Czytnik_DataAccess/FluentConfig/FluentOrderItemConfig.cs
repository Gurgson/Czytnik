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
    public class FluentOrderItemConfig : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> modelBuilder)
        {
            modelBuilder.Property(i => i.OrderId).IsRequired();
            modelBuilder.Property(i => i.BookId).IsRequired();
            modelBuilder.Property(i => i.Price).IsRequired().HasColumnType("decimal(18,2)");
            modelBuilder.Property(i => i.Quantity).HasDefaultValue(1);

            modelBuilder.HasKey(i => new { i.BookId, i.OrderId });
            modelBuilder.HasOne(i => i.Book).WithMany(i => i.OrderItems).HasForeignKey(i => i.BookId);
            modelBuilder.HasOne(i => i.Order).WithMany(i => i.OrderItems).HasForeignKey(i => i.OrderId);
        }
    }
}
