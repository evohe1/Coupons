using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteDeals.Core.Model
{
    public class ProductTags
    {
        public int Id { get; set; }
        public int? ProductId { get; set; }
        public int? TagId { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
    }
}
