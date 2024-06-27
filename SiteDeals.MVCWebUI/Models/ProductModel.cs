using SiteDeals.Core.Model;

namespace SiteDeals.MVCWebUI.Models
{
    public class ProductModel
    {
        public Product Product { get; set; }

        public IEnumerable<string> ProductImages{ get; set; }

        public string DateRelative { get; set; }

        public IEnumerable<ProductReviews> ProductReviews { get; set; }

        public IEnumerable<Tag> Tags { get; set; }

        public isLiked IsLiked { get; set; }
    }

    public enum isLiked
    {
        unknown = 0,
        liked = 1,
        disliked = 2
    }
}
