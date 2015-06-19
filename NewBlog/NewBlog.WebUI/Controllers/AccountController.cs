using NewBlog.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;

namespace NewBlog.WebUI.Controllers
{
    public class AccountController : Controller
    {
        
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Login loginData, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (WebSecurity.Login(loginData.Username, loginData.Password, loginData.RememberMe))
                {
                    if (returnUrl != null)
                    {
                        return Redirect(returnUrl); 
                    }
                    return RedirectToAction("Posts", "Blog");
                }
                else
                {
                    ModelState.AddModelError("", "Sorry, the username or password is invalid");
                    return View(loginData);
                }
            }

            ModelState.AddModelError("", "Sorry, the username or password is invalid");
            return View(loginData);
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Register registerData)
        {
            bool createdUser = false;
            string errMessage = string.Empty;

            if (ModelState.IsValid)
            {
                try
                {
                    WebSecurity.CreateUserAndAccount(registerData.Username, registerData.Password);
                    Roles.AddUserToRole(registerData.Username, "user");
                    createdUser = true;
                    return RedirectToAction("Posts", "Blog");
                    
                }
                catch (MembershipCreateUserException exception)
                {
                    ModelState.AddModelError("", "Sorry, the username is already exists");
                    createdUser = false;
                    return View(registerData);
                }

                if (createdUser)
                {
                    if (WebSecurity.Login(registerData.Username, registerData.Password))
                        return RedirectToAction("Posts", "Blog");
                    else
                        ModelState.AddModelError("", "Error logging user in with that username/password.");

                }
                else
                {
                    ModelState.AddModelError("", errMessage);
                }
            }

            ModelState.AddModelError("", "Sorry, the username is already exists");
            return View(registerData);
        }

        public ActionResult Logout()
        {
            WebSecurity.Logout();
            return RedirectToAction("Posts", "Blog");
        }

    }
}