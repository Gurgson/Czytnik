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
    public class FluentLanguageConfig : IEntityTypeConfiguration<Language>
    {
        public void Configure(EntityTypeBuilder<Language> modelBuilder)
        {
            modelBuilder.HasIndex(i => i.Name).IsUnique();
            modelBuilder.Property(i => i.Id).HasMaxLength(2);
            modelBuilder.Property(i => i.Name).HasMaxLength(50);
            
        }
    }
}
