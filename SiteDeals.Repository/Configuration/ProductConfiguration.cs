using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SiteDeals.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteDeals.Repository.Configuration
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Price).IsRequired();
            builder.Property(x => x.IsDeleted).IsRequired();
            builder.Property(x => x.Link).IsRequired();
            builder.Property(x => x.Likes).IsRequired();
            builder.Property(x=>x.CreatedAt).IsRequired();
            builder.Property(x => x.CreatedById).IsRequired();
            builder.HasOne(x => x.User).WithMany().HasForeignKey(x => x.CreatedById);
            //builder.HasMany(x => x.Images).WithOne().HasForeignKey(x => x.ProductId);
            //builder.HasMany(x => x.Tags).WithOne().HasForeignKey(x => x.ProductId);


        }
    }
}
