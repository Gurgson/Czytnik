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
    public class FluentBookTranslatorConfig : IEntityTypeConfiguration<BookTranslator>
    {
        public void Configure(EntityTypeBuilder<BookTranslator> modelBuilder)
        {
            modelBuilder.HasKey(i => new { i.BookId, i.TranslatorId });
            modelBuilder.HasOne(i => i.Book).WithMany(i => i.BookTranslators).HasForeignKey(i => i.BookId);
            modelBuilder.HasOne(i => i.Translator).WithMany(i => i.BookTranslators).HasForeignKey(i => i.TranslatorId);
        }
    }
}
