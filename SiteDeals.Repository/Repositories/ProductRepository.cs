using SiteDeals.Core.Model;
using SiteDeals.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SiteDeals.Repository.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<ProductReviews>> GetAllReviews(int productId, int page, int count)
        {
            return _context.ProductReviews.Where(x => x.ProductId == productId).OrderByDescending(x => x.Likes).Skip(count * (page - 1)).Take(count).ToList();
        }

        public async Task<IAsyncEnumerable<Product>> GetProductsForHome(int count)
        {
            return _context.Products.Include(x => x.User).Include(x => x.Images).OrderByDescending(x => x.CreatedAt).AsAsyncEnumerable();
        }

        public async Task<Product> GetById(int id)
        {
            return await _context.Products.Include(x => x.User).Where(x => x.Id == id).FirstOrDefaultAsync().ConfigureAwait(false);
        }

        public async override Task<Product> GetByIdAsync(int id)
        {
            return _context.Products.Include(x => x.User).Include(x => x.Images).Where(x => x.Id == id).FirstOrDefault();
        }

    }
}
