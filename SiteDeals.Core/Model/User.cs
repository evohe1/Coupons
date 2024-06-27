//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace SiteDeals.Core.Model
//{
//    public  class User
//    {
//        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
//        public int Id { get; set; }

//        [Display(Name = "UserName")]
//        [Required(AllowEmptyStrings = false, ErrorMessage = "UserName required")]
//        public string? UserName { get; set; }

//        [Display(Name = "Email")]
//        [Required(AllowEmptyStrings = false, ErrorMessage = "Email required")]
//        [DataType(DataType.EmailAddress)]
//        public string? Email { get; set; }

//        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required")]
//        [DataType(DataType.Password)]
//        [MinLength(6, ErrorMessage = "Minimum 6 characters required")]
//        public string? PasswordHash { get; set; }
        
//        public DateTime CreatedAt { get; set; }
//        public bool IsDeleted { get; set; }
//        public string? AvatarUrl { get; set; }

        

//        public ICollection<Product>? Products { get; set; }
//    }
//}
