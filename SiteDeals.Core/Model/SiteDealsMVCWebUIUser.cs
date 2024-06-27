using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace SiteDeals.MVCWebUI.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the SiteDealsMVCWebUIUser class
    [Table("AspNetUsers")]
    public class SiteDealsMVCWebUIUser : IdentityUser <int>
    {
        public string? City { get; set; }
        public string? Picture { get; set; }
        public DateTime? BirthDate { get; set; }
        public int? Gender { get; set; }
        public DateTime? CreatedAt { get; set; }

    }
}
