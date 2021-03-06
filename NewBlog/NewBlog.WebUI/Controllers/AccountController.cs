﻿using Microsoft.Security.Application;
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
        [ValidateInput(false)]
        public ActionResult Login(Login loginData, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Sorry, the username or password is invalid");
                return View(loginData);
            }

            loginData.Username = Sanitizer.GetSafeHtmlFragment(loginData.Username);
            loginData.Password = Sanitizer.GetSafeHtmlFragment(loginData.Password);


            if (WebSecurity.Login(loginData.Username, loginData.Password, loginData.RememberMe))
            {
                if (returnUrl != null)
                    return Redirect(returnUrl); 

                return RedirectToAction("Posts", "Blog");
            }
            else
            {
                ModelState.AddModelError("", "Sorry, the username or password is invalid");
                return View(loginData);
            }            
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
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Sorry, the username is already exists");
                return View(registerData);
            }

            registerData.Username = Sanitizer.GetSafeHtmlFragment(registerData.Username);
            registerData.Password = Sanitizer.GetSafeHtmlFragment(registerData.Password);

            try
            {
                WebSecurity.CreateUserAndAccount(registerData.Username, registerData.Password);
                Roles.AddUserToRole(registerData.Username, "user");

                if (WebSecurity.Login(registerData.Username, registerData.Password))
                    return RedirectToAction("Posts", "Blog");
                else
                    ModelState.AddModelError("", "Error logging user in with that username/password.");
                return View(registerData);
                    
            }
            catch (MembershipCreateUserException exception)
            {
                ModelState.AddModelError("", "Sorry, the username is already exists");
                return View(registerData);
            }
        }

        public ActionResult Logout()
        {
            WebSecurity.Logout();
            return RedirectToAction("Posts", "Blog");
        }

    }
}