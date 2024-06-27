using Microsoft.AspNetCore.Identity;
using SiteDeals.MVCWebUI.Areas.Identity.Data;

namespace SiteDeals.MVCWebUI.CustomValidations
{
    public class CustomUserValidator : IUserValidator<SiteDealsMVCWebUIUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<SiteDealsMVCWebUIUser> manager, SiteDealsMVCWebUIUser user)
        {
            List<IdentityError> errors = new List<IdentityError>();
            string[] Digits = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
            foreach (var item in Digits)
            {
                if (user.UserName[0].ToString() == item)
                {
                    errors.Add(new IdentityError()
                    {
                        Code = "UserNameContainsFirstLetterDigitCharacters",
                        Description = "Kullancı Adının ilk karakteri sayı olamaz"
                    });
                }
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