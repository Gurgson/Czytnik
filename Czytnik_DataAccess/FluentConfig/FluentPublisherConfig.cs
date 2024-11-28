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
    public class FluentPublisherConfig : IEntityTypeConfiguration<Publisher>
    {
        public void Configure(EntityTypeBuilder<Publisher> modelBuilder)
        {
            modelBuilder.HasIndex(i => i.Name).IsUnique();
            modelBuilder.Property(i => i.Name).IsRequired().HasMaxLength(50);
        }
    }
}
