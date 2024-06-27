using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;
using SiteDeals.MVCWebUI.Areas.Identity.Data;

namespace SiteDeals.MVCWebUI.CustomTagHelpers
{
    [HtmlTargetElement("td", Attributes = "user-roles")]
    public class UserRolesName:TagHelper
    {
        public UserManager<SiteDealsMVCWebUIUser> UserManager { get; set; }

        public UserRolesName(UserManager<SiteDealsMVCWebUIUser> userManager)
        {
            this.UserManager = userManager;
        }

        [HtmlAttributeName("user-roles")]
        public string UserId { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            SiteDealsMVCWebUIUser user = await UserManager.FindByIdAsync(UserId);

            IList<string> roles = await UserManager.GetRolesAsync(user);

            string html = string.Empty;

            roles.ToList().ForEach(x =>
            {
                html += $"<span class='badge badge-info'>  {x}  </span>";
            });

            output.Content.SetHtmlContent(html);
        }
    }
}
