using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Business.Services;
using Core.Concrete.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Web.Models;

namespace Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // LOGIN
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToAction("Index", "Dashboard");

                default:
                    ModelState.AddModelError("", "Hatalı giriş.");
                    return View(model);
            }
        }

        // REGISTER
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
            var result = await UserManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await SignInManager.SignInAsync(user, false, false);
                return RedirectToAction("Index", "Home");
            }

            AddErrors(result);
            return View(model);
        }

        // 🔥 FORGOT PASSWORD
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await UserManager.FindByEmailAsync(model.Email);

            if (user == null)
                return RedirectToAction("ForgotPasswordConfirmation");

            var code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);

            var callbackUrl = Url.Action("ResetPassword", "Account",
                new { userId = user.Id, code = code },
                protocol: Request.Url.Scheme);

            // 🔥 TEST İÇİN
            System.Diagnostics.Debug.WriteLine("RESET LINK:");
            System.Diagnostics.Debug.WriteLine(callbackUrl);

            return RedirectToAction("ForgotPasswordConfirmation");
        }

        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        // 🔥 RESET PASSWORD
        [AllowAnonymous]
        public ActionResult ResetPassword(string userId, string code)
        {
            if (code == null)
                return View("Error");

            return View(new ResetPasswordViewModel1 { Code = code });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await UserManager.FindByEmailAsync(model.Email);

            if (user == null)
                return RedirectToAction("ResetPasswordConfirmation");

            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);

            if (result.Succeeded)
                return RedirectToAction("ResetPasswordConfirmation");

            AddErrors(result);
            return View();
        }

        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        // LOGOUT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        // HELPERS
        private IAuthenticationManager AuthenticationManager
        {
            get { return HttpContext.GetOwinContext().Authentication; }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError("", error);
        }

        internal class ChallengeResult : ActionResult
        {
            private string provider;
            private string v1;
            private string v2;

            public ChallengeResult(string provider, string v1, string v2)
            {
                this.provider = provider;
                this.v1 = v1;
                this.v2 = v2;
            }

            public override void ExecuteResult(ControllerContext context)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}