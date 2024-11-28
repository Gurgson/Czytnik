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
    public class FluentFavouriteConfig : IEntityTypeConfiguration<Favourite>
    {
        public void Configure(EntityTypeBuilder<Favourite> modelBuilder)
        {
            modelBuilder.HasOne(i => i.Book).WithMany(i => i.Favourites).HasForeignKey(i => i.BookId);
        }
    }
}
