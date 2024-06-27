using System.ComponentModel.DataAnnotations;

namespace SiteDeals.MVCWebUI.Models
{
    public class LoginViewModel
    {
        [Display(Name ="Email Adresi Veya Kullanıcı Adınız")]
        public string UsernameOrEmail { get; set; }
        [Display(Name ="Şifreniz")]
        [Required(ErrorMessage ="Şifre Alanı Gereklidir.")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage ="Şifreniz en az 6 karakterli olmalıdır.")]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
