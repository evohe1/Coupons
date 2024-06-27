using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteDeals.Core.Model
{
    public class ProductReviews
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
        public int Likes { get; set; }
        public string? IpAddress { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Review { get; set; }
        public bool Approved { get; set; }

        [NotMapped]
        public string DateRelative { get; set; }


    }
}
