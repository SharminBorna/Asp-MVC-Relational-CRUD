using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using work_01.ViewModels;

namespace work_01.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(RegisterVM vm)
        {
            if (ModelState.IsValid)
            {
                var userStore = new UserStore<IdentityUser>();
                var userManager = new UserManager<IdentityUser>(userStore);
                var user = new IdentityUser { UserName = vm.Username };
                var result = userManager.Create(user, vm.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    ModelState.AddModelError("", "Register Failed!!");
                    return View(vm);
                }
            }
            return View(vm);
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginVM vm)
        {
            if (ModelState.IsValid)
            {
                var userStore = new UserStore<IdentityUser>();
                var userManager = new UserManager<IdentityUser>(userStore);
                var user = userManager.Find(vm.Username, vm.Password);
                if (user != null)
                {
                    var authManager = System.Web.HttpContext.Current.GetOwinContext().Authentication;
                    authManager.SignIn(new AuthenticationProperties() { }, userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie));
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Login Failed!!");
                    return View(vm);
                }
            }
            return View(vm);
        }
        [Authorize]
        public ActionResult Logout()
        {
            var authManager = System.Web.HttpContext.Current.GetOwinContext().Authentication;
            authManager.SignOut();
            return RedirectToAction("Login", "Account");
        }
    }
}