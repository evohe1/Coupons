using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteDeals.Core.Model
{
    public class ProductImages
    {
        public int Id { get; set; }
        public int? ProductId { get; set; }
        public string CloudinaryPublicId { get; set; }
        public string Url { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
