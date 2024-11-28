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
    public class FluentBookAuthorConfig : IEntityTypeConfiguration<BookAuthor>
    {
        public void Configure(EntityTypeBuilder<BookAuthor> modelBuilder)
        {
            modelBuilder.HasKey(i => new { i.BookId, i.AuthorId });
            modelBuilder.HasOne(i => i.Book).WithMany(i => i.BookAuthors).HasForeignKey(i => i.BookId);
            modelBuilder.HasOne(i => i.Author).WithMany(i => i.BookAuthors).HasForeignKey(i => i.AuthorId);
        }
    }
}
