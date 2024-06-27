using Microsoft.EntityFrameworkCore;
using SiteDeals.Core.Model;
using System.Reflection;

namespace SiteDeals.Repository
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {

        }
        public DbSet<Product> Products { get; set; }
        //public DbSet<User> Users { get; set; }
        public DbSet<ProductReviews> ProductReviews { get; set; }
        public DbSet<ProductLikes> ProductLikes { get; set; }
        public DbSet<ProductReviewLikes> ProductReviewLikes { get; set; }
        public DbSet<ProductImages> ProductImages { get; set; }
        public DbSet<ProductTags> ProductTags { get; set; }

        public DbSet<Tag> Tags { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());



            base.OnModelCreating(modelBuilder);
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        optionsBuilder.UseSqlServer("Data Source=185.98.61.4;Initial Catalog=DealSite;User ID=sitedeal;Password=sitedeal1;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        //    }
        //}


    }


}
