using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteDeals.Core.Model
{
    public class ProductReviewLikes
    {
        public int Id { get; set; }
        public int ProductReviewId { get; set; }
        public int UserId { get; set; }
        public bool Liked { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
