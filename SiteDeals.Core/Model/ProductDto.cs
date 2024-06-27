using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteDeals.Core.Model
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public Double Price { get; set; }
        public string? Link { get; set; }
        public int CreatedById { get; set; }
    }

}
