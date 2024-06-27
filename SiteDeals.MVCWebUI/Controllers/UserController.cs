using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SiteDeals.Core.Model;
using SiteDeals.Repository;
using SiteDeals.Repository.Repositories;
using System.Text;
using SiteDeals.MVCWebUI.Models;
using SiteDeals.MVCWebUI.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using SiteDeals.Core.Helper;
using Microsoft.AspNetCore.Authorization;
using Mapster;
using Microsoft.AspNetCore.Mvc.Rendering;
using SiteDeals.MVCWebUI.Enums;
using MassTransit;
using static sun.awt.image.ImageWatched;
using System.Net.Mail;
using Messages.Shared;
using MassTransit.Transports;
using Newtonsoft.Json;

namespace SiteDeals.MVCWebUI.Controllers
{
    public class UserController : Controller
    {
        public UserManager<SiteDealsMVCWebUIUser> userManager { get; }
        public SignInManager<SiteDealsMVCWebUIUser> signInManager { get; }

        private ISendEndpointProvider _sendEndpointProvider;

        public UserController(UserManager<SiteDealsMVCWebUIUser> userManager, SignInManager<SiteDealsMVCWebUIUser> signInManager, ISendEndpointProvider sendEndpointProvider)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this._sendEndpointProvider = sendEndpointProvider;
        }
        // GET: UserController
        public ActionResult Index()
        {
            return View();
        }

        // GET: UserController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UserController/Create
        public ActionResult Signup()
        {
            return View();
        }

        //[HttpPost]
        //public async Task<ActionResult> Signup(User user)
        //{
        //    var isEmailexist = isEmailExist(user.Email);
        //    var isUserexist = isUserNameExist(user.UserName);

        //    if (isUserexist)
        //    {
        //        ViewBag.ErrorUser = "This Username already taken";
        //        return View();
        //    }
        //    else if (isEmailexist)
        //    {
        //        ViewBag.ErrorEmail = "This Email already registered";
        //        return View();
        //    }
        //    else
        //    {
        //        user.CreatedAt = DateTime.Now;
        //        user.PasswordHash = await Hash(user.PasswordHash).ConfigureAwait(false);
        //        await _userRepository.AddAsync(user).ConfigureAwait(false);
        //        return RedirectToAction("Login");
        //    }
        //}


        public ActionResult Login(string ReturnUrl)
        {
            TempData["ReturnUrl"] = ReturnUrl;
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel userlogin)
        {
            if (ModelState.IsValid)
            {
                SiteDealsMVCWebUIUser userMail = await userManager.FindByEmailAsync(userlogin.UsernameOrEmail);
                SiteDealsMVCWebUIUser userName = await userManager.FindByNameAsync(userlogin.UsernameOrEmail);
                if (userMail == null && userName != null)
                {
                    if (await userManager.IsLockedOutAsync(userName))
                    {
                        ModelState.AddModelError("", "Hesabınız bir süreliğine kilitlenmiştir lütfen daha sonra tekrar deneyiniz.");
                        return View();
                    }
                    await signInManager.SignOutAsync();
                    Microsoft.AspNetCore.Identity.SignInResult result = await signInManager.PasswordSignInAsync(userName, userlogin.Password, userlogin.RememberMe, false);
                    if (result.Succeeded)
                    {
                        await userManager.ResetAccessFailedCountAsync(userName);
                        if (TempData["ReturnUrl"] != null)
                        {
                            return Redirect(TempData["ReturnUrl"].ToString());
                        }
                        return RedirectToAction("Index", "Admin");
                    }
                    else
                    {
                        await userManager.AccessFailedAsync(userName);
                        int fail = await userManager.GetAccessFailedCountAsync(userName);
                        if (fail >= 3)
                        {
                            ModelState.AddModelError("", $"{fail} kez başarısız giriş");
                        }

                        if (fail == 5)
                        {
                            await userManager.SetLockoutEndDateAsync(userName, new System.DateTimeOffset(DateTime.Now.AddMinutes(15)));
                            ModelState.AddModelError("", "Hesabınız 5 başarısız girişten dolayı 15 dakika süreyle kiltlenmiştir. Lütfen daha sonra tekrar deneyiniz");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Geçersiz bilgiler bulunmaktadır.");
                        }

                    }
                }
                else if (userMail != null && userName == null)
                {
                    if (await userManager.IsLockedOutAsync(userMail))
                    {
                        ModelState.AddModelError("", "Hesabınız bir süreliğine kilitlenmiştir lütfen daha sonra tekrar deneyiniz.");
                        return View();
                    }
                    await signInManager.SignOutAsync();
                    Microsoft.AspNetCore.Identity.SignInResult result = await signInManager.PasswordSignInAsync(userMail, userlogin.Password, userlogin.RememberMe, false);
                    if (result.Succeeded)
                    {
                        await userManager.ResetAccessFailedCountAsync(userMail);
                        if (TempData["ReturnUrl"] != null)
                        {
                            return Redirect(TempData["ReturnUrl"].ToString());
                        }
                        return RedirectToAction("Index", "Admin");
                    }
                    else
                    {
                        await userManager.AccessFailedAsync(userMail);
                        int fail = await userManager.GetAccessFailedCountAsync(userMail);
                        if (fail >= 3)
                        {
                            ModelState.AddModelError("", $"{fail} kez başarısız giriş");
                        }
                        if (fail == 5)
                            if (fail == 5)
                            {
                                await userManager.SetLockoutEndDateAsync(userMail, new System.DateTimeOffset(DateTime.Now.AddMinutes(15)));
                                ModelState.AddModelError("", "Hesabınız 5 başarısız girişten dolayı 15 dakika süreyle kiltlenmiştir. Lütfen daha sonra tekrar deneyiniz");
                            }
                            else
                            {
                                ModelState.AddModelError("", "Geçersiz bilgiler bulunmaktadır.");
                            }
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Bu kullanıcı adına veya bu email adresine kayıtlı kullanıcı bulunmamaktadır.");
                }

            }

            return View();
        }

        public IActionResult ResetPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(PasswordResetViewModel passwordResetViewModel)
        {
            SiteDealsMVCWebUIUser user = userManager.FindByEmailAsync(passwordResetViewModel.Email).Result;
            ViewBag.status = true;
            if (user != null)
            {
                string passwordResetToken = userManager.GeneratePasswordResetTokenAsync(user).Result;
                string passwordResetLink = Url.Action("ResetPasswordConfirm", "User", new
                {
                    userId = user.Id,
                    token = passwordResetToken
                }, HttpContext.Request.Scheme);
                //PasswordReset.PasswordResetSendEmail(passwordResetLink, passwordResetViewModel.Email); 
     

                var mail = new SendEmail()
                {
                    Subject = "www.kampanyan.com::Şifre Sıfırlama",
                    From = "iletisim@kampanyan.com",
                    To = passwordResetViewModel.Email
                };
                mail.Body = "<h2>Şifrenizi yenilemek için lütfen aşağıdaki linke tıklayınız.</h2><hr/>";
                mail.Body += $"<a href={passwordResetLink}>Şifre yenileme linki</a>";

                var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:scheduled-jobs"));
                await endpoint.Send(mail);
            }
            //else
            //{
            //    ModelState.AddModelError("", "Sistemde kayıtlı Email adresi bulunamamıştır.");
            //}
            return View(passwordResetViewModel);
        }
        public IActionResult ResetPasswordConfirm(string userId, string token)
        {
            TempData["userId"] = userId;
            TempData["token"] = token;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPasswordConfirm([Bind("PasswordNew")] PasswordResetViewModel passwordResetViewModel)
        {
            string token = TempData["token"].ToString();
            string userId = TempData["userId"].ToString();
            SiteDealsMVCWebUIUser user = await userManager.FindByIdAsync(userId);
            if (user != null)
            {
                IdentityResult result = await userManager.ResetPasswordAsync(user, token, passwordResetViewModel.PasswordNew);
                if (result.Succeeded)
                {
                    await userManager.UpdateSecurityStampAsync(user);

                    TempData["passwordResetInfo"] = "şifreniz başarıyla yenilenmiştir.Yeni şifreniz ile giriş yapabilirsiniz.";
                    ViewBag.status = "success";



                }

                else
                {
                    ModelState.AddModelError("", "Hata meydana gelmiştir.");
                }

            }
            return View();
        }


        [Authorize]
        public IActionResult Info()
        {
            SiteDealsMVCWebUIUser user = userManager.FindByNameAsync(User.Identity.Name).Result;
            UserViewModel userViewModel = user.Adapt<UserViewModel>();
            return View(userViewModel);
        }
        public IActionResult PasswordChange()
        {
            return View();
        }

        [HttpPost]
        public IActionResult PasswordChange(PasswordChangeViewModel passwordChangeViewModel)
        {
            if (ModelState.IsValid)
            {
                SiteDealsMVCWebUIUser user = userManager.FindByNameAsync(User.Identity.Name).Result;


                bool exist = userManager.CheckPasswordAsync(user, passwordChangeViewModel.PasswordOld).Result;
                if (exist)
                {
                    IdentityResult result = userManager.ChangePasswordAsync(user, passwordChangeViewModel.PasswordOld, passwordChangeViewModel.PasswordNew).Result;
                    if (result.Succeeded)
                    {
                        userManager.UpdateSecurityStampAsync(user);
                        signInManager.SignOutAsync();
                        signInManager.PasswordSignInAsync(user, passwordChangeViewModel.PasswordNew, true, false);
                        ViewBag.success = "true";
                    }
                    else
                    {
                        foreach (var item in result.Errors)
                        {
                            ModelState.AddModelError("", item.Description);
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Eski şifreniz yanlış");
                }


            }
            return View(passwordChangeViewModel);
        }

        public IActionResult UserEdit()
        {
            SiteDealsMVCWebUIUser user = userManager.FindByNameAsync(User.Identity.Name).Result;
            UserViewModel userViewModel = user.Adapt<UserViewModel>();
            ViewBag.Gender = new SelectList(Enum.GetNames(typeof(Gender)));
            return View(userViewModel);

        }

        [HttpPost]
        public async Task<IActionResult> UserEdit(UserViewModel userViewModel, IFormFile Picture)
        {
            ModelState.Remove("Password");



            ViewBag.Gender = new SelectList(Enum.GetNames(typeof(Gender)));
            if (ModelState.IsValid)
            {
                SiteDealsMVCWebUIUser user = await userManager.FindByNameAsync(User.Identity.Name);
                if (Picture != null && Picture.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(Picture.FileName);
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/UserPicture", fileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await Picture.CopyToAsync(stream);
                        user.Picture = "/images/UserPicture/" + fileName;
                    }

                }

                user.UserName = userViewModel.UserName;
                user.Email = userViewModel.Email;
                user.City = userViewModel.City;
                user.BirthDate = userViewModel.BirthDate;
                user.Gender = (int)userViewModel.Gender;
                IdentityResult result = await userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    await userManager.UpdateSecurityStampAsync(user);
                    await signInManager.SignOutAsync();
                    await signInManager.SignInAsync(user, true);
                    ViewBag.success = "true";
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
            }
            return View(userViewModel);
        }

        public void LogOut()
        {
            signInManager.SignOutAsync();
        }
        //[NonAction]
        //public bool isEmailExist(string email)
        //{
        //    var v = _userRepository.Where(u => u.Email == email).FirstOrDefault();
        //    return v != null;
        //}

        //[NonAction]
        //public bool isUserNameExist(string username)
        //{
        //    var v = _userRepository.Where(u => u.UserName == username).FirstOrDefault();
        //    return v != null;
        //}

        public async Task<string> Hash(string text)
        {
            var salt = Encoding.UTF8.GetBytes("saltBaeNusret");
            var hash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: text,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8));
            return hash;
        }
    }
}
