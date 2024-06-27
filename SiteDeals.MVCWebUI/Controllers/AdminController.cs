using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SiteDeals.MVCWebUI.Areas.Identity.Data;
using SiteDeals.MVCWebUI.Models;

namespace SiteDeals.MVCWebUI.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private UserManager<SiteDealsMVCWebUIUser> _userManager { get; }
        private RoleManager<SiteDealsMVCWebIRole> _roleManager { get; }
        private SignInManager<SiteDealsMVCWebUIUser> _signInManager { get; }
        public AdminController(UserManager<SiteDealsMVCWebUIUser> userManager,SignInManager<SiteDealsMVCWebUIUser> signInManager, RoleManager<SiteDealsMVCWebIRole> roleManager=null)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }
        public IActionResult Index()
        {
            return View(_userManager.Users.ToList());
        }
        public IActionResult Users()
        {
            return View(_userManager.Users.ToList());
        }
        public IActionResult RoleCreate()
        {
            return View();
        }
        [HttpPost]
        public IActionResult RoleCreate(RoleViewModel roleViewModel)
        {
            SiteDealsMVCWebIRole role = new SiteDealsMVCWebIRole();
            role.Name = roleViewModel.Name;
           
            IdentityResult result =_roleManager.CreateAsync(role).Result;
            if (result.Succeeded)
            {
                return RedirectToAction("Roles");

            }
            else
            {
                ModelState.AddModelError("",result.Errors.ToString());
            }
            return View(roleViewModel);
        }
        public IActionResult Roles()
        {
            return View(_roleManager.Roles.ToList());
        }
        public IActionResult RoleDelete(string id)
        {
            SiteDealsMVCWebIRole role = _roleManager.FindByIdAsync(id).Result;
            if (role != null)
            {
                IdentityResult result = _roleManager.DeleteAsync(role).Result;
            }

            return RedirectToAction("Roles");
        }
        public IActionResult RoleUpdate(string id)
        {
            SiteDealsMVCWebIRole role = _roleManager.FindByIdAsync(id).Result;

            if (role != null)
            {
                return View(role.Adapt<RoleViewModel>());
            }

            return RedirectToAction("Roles");
        }

        [HttpPost]
        public IActionResult RoleUpdate(RoleViewModel roleViewModel)
        {
            SiteDealsMVCWebIRole role = _roleManager.FindByIdAsync(roleViewModel.Id).Result;

            if (role != null)
            {
                role.Name = roleViewModel.Name;
                IdentityResult result = _roleManager.UpdateAsync(role).Result;

                if (result.Succeeded)
                {
                    return RedirectToAction("Roles");
                }
                else
                {
                    ModelState.AddModelError("", result.Errors.ToString());
                }
            }
            else
            {
                ModelState.AddModelError("", "Güncelleme işlemi başarısız oldu.");
            }

            return View(roleViewModel);
        }

        public IActionResult RoleAssign(string id)
        {
            TempData["userId"] = id;
            SiteDealsMVCWebUIUser user = _userManager.FindByIdAsync(id).Result;

            ViewBag.userName = user.UserName;

            IQueryable<SiteDealsMVCWebIRole> roles = _roleManager.Roles;

            List<string> userroles = _userManager.GetRolesAsync(user).Result as List<string>;

            List<RoleAssignViewModel> roleAssignViewModels = new List<RoleAssignViewModel>();

            foreach (var role in roles)
            {
                RoleAssignViewModel r = new RoleAssignViewModel();
                r.RoleId = role.Id.ToString();
                r.RoleName = role.Name;
                if (userroles.Contains(role.Name))
                {
                    r.Exist = true;
                }
                else
                {
                    r.Exist = false;
                }
                roleAssignViewModels.Add(r);
            }

            return View(roleAssignViewModels);
        }

        [HttpPost]
        public async Task<IActionResult> RoleAssign(List<RoleAssignViewModel> roleAssignViewModels)
        {
            SiteDealsMVCWebUIUser user = _userManager.FindByIdAsync(TempData["userId"].ToString()).Result;

            foreach (var item in roleAssignViewModels)
            {
                if (item.Exist)

                {
                    await _userManager.AddToRoleAsync(user, item.RoleName);
                }
                else
                {
                    await _userManager.RemoveFromRoleAsync(user, item.RoleName);
                }
            }

            return RedirectToAction("Users");
        }

        public async Task<IActionResult> ResetUserPassword(string id)
        {
            SiteDealsMVCWebUIUser user =await _userManager.FindByIdAsync(id);
            PasswordResetViewAdminModel passwordResetViewAdminModel = new PasswordResetViewAdminModel();
            passwordResetViewAdminModel.UserId = user.Id.ToString();
            return View(passwordResetViewAdminModel);
        }
        [HttpPost]
        public async Task<IActionResult> ResetUserPassword(PasswordResetViewAdminModel passwordResetViewAdminModel)
        {
            SiteDealsMVCWebUIUser user = await _userManager.FindByIdAsync(passwordResetViewAdminModel.UserId);

            string token = await _userManager.GeneratePasswordResetTokenAsync(user);

            await _userManager.ResetPasswordAsync(user, token, passwordResetViewAdminModel.NewPassword);

            await _userManager.UpdateSecurityStampAsync(user);

            

            return RedirectToAction("Users");
        }
    }
}
