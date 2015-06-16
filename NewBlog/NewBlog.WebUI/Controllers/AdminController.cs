using NewBlog.Domain.Abstract;
using NewBlog.Domain.Entities;
using NewBlog.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NewBlog.WebUI.Controllers
{
    public class AdminController : Controller
    {
        IAdminRepository _adminRepository;

        public AdminController(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        public ViewResult List()
        {
            return View(_adminRepository.Posts());
        }

        public ViewResult Edit(int id)
        {
            Post post = _adminRepository.Posts()
                .FirstOrDefault(g => g.Id == id);

            return View(post);
        }
    }
}