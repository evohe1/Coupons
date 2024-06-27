using SiteDeals.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteDeals.Core.Services
{
    public interface IProductService:IService<Product>
    {

       Task<string> GetLink(int id);
        Task<string> GetLinkHepsiburada(int id);

        public Task<IEnumerable<ProductReviews>> GetProductReviewsAsync(int productId, int page, int count);

    }
}
