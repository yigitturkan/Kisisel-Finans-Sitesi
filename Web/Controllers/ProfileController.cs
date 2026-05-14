using Business.Services;
using Core.Concrete.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private ApplicationUserManager _userManager;

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

        // GET: Profile/Index
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var user = UserManager.FindById(userId);

            if (user == null)
                return HttpNotFound();

            // Navbar için
            string fullName = (user.FirstName + " " + user.LastName).Trim();
            ViewBag.UserName = !string.IsNullOrEmpty(fullName) ? fullName : user.UserName;
            ViewBag.ProfileImage = user.ProfilePicturePath;

            return View(user);
        }

        // GET: Profile/Edit
        public ActionResult Edit()
        {
            var userId = User.Identity.GetUserId();
            var user = UserManager.FindById(userId);

            if (user == null)
                return HttpNotFound();

            return View(user);
        }

        // POST: Profile/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ApplicationUser model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = UserManager.FindById(model.Id);
                    if (user != null)
                    {
                        user.FirstName = model.FirstName;
                        user.LastName = model.LastName;
                        user.DateOfBirth = model.DateOfBirth;

                        var result = UserManager.Update(user);
                        if (result.Succeeded)
                        {
                            TempData["Success"] = "Profil başarıyla güncelleştirildi!";
                            return RedirectToAction("Index");
                        }

                        foreach (var error in result.Errors)
                            ModelState.AddModelError("", error);
                    }
                }
                catch (System.Exception ex)
                {
                    ModelState.AddModelError("", "Hata: " + ex.Message);
                }
            }
            return View(model);
        }
    }
}