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
    public class FluentDiscountConfig : IEntityTypeConfiguration<Discount>
    {
        public void Configure(EntityTypeBuilder<Discount> modelBuilder)
        {
            modelBuilder.HasIndex(i => i.Name).IsUnique();
            modelBuilder.Property(i => i.Name).IsRequired().HasMaxLength(150);
            modelBuilder.Property(i => i.DiscountValue).IsRequired();
        }
    }
}
