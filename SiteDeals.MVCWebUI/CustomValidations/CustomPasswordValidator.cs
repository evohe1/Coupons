using Microsoft.AspNetCore.Identity;
using SiteDeals.MVCWebUI.Areas.Identity.Data;

namespace SiteDeals.MVCWebUI.CustomValidations
{
    public class CustomPasswordValidator : IPasswordValidator<SiteDealsMVCWebUIUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<SiteDealsMVCWebUIUser> manager, SiteDealsMVCWebUIUser user, string password)
        {
            List<IdentityError> errors = new List<IdentityError>();
            if (password.ToLower().Contains(user.UserName.ToLower()))
            {
                if (!user.Email.Contains(user.UserName))
                {
                    errors.Add(new IdentityError() { Code = "PasswordContainsUserName", Description = "Şifre Alanı Kullancı Adı İçeremez." });
                }
             
            }
            if (password.ToLower().Contains(user.Email.ToLower()))
            {
                errors.Add(new IdentityError() { Code = "EmailContainsUserName", Description = "Şifre Alanı Email İçeremez." });

            }
            if (errors.Count == 0)
            {
                return Task.FromResult(IdentityResult.Success);
            }
            else
            {
                return Task.FromResult(IdentityResult.Failed(errors.ToArray()));
            }

        }
    }
}
