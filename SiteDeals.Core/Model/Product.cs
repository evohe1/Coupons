using SiteDeals.MVCWebUI.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteDeals.Core.Model
{
    public class Product
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public Double  PriceWithoutDiscount { get; set; }
        public Double Price { get; set; }
        public Double PriceAbroad { get; set; }
        public Boolean IsDeleted { get; set; }
        public string? Link { get; set; }
        public int Likes { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedById { get; set; }
        public string? CloudinaryPublicId { get; set; }
        public int Status { get; set; }
        public int Reviews { get; set; }
        public string CreatedByUserName { get; set; }
        public string Vendor { get; set; }

        public SiteDealsMVCWebUIUser? User { get; set; }
        public List<ProductImages>? Images { get; set; }
        public List<ProductTags>? Tags { get; set; }
        //public Category Category { get; set; }
    }
}
