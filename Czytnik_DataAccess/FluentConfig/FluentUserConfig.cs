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
    public class FluentUserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> modelBuilder)
        {
            modelBuilder.Property(i => i.FirstName).HasMaxLength(30);
            modelBuilder.Property(i => i.SecondName).HasMaxLength(30);
            modelBuilder.Property(i => i.Surname).HasMaxLength(40);

            modelBuilder.HasMany(u => u.Favourites).WithOne(f => f.User).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.HasMany(u => u.CartItems).WithOne(f => f.User).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.HasMany(u => u.Reviews).WithOne(f => f.User).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.HasMany(u => u.Orders).WithOne(f => f.User).OnDelete(DeleteBehavior.SetNull);

        }
    }
}
