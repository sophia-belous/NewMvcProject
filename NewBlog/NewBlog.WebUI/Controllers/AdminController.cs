using NewBlog.Domain.Abstract;
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
        IBlogRepository _blogRepository;

        public AdminController(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        public ViewResult List(int p = 1)
        {
            var viewModel = new ListViewModel(_blogRepository, p);
            return View(viewModel);
        }

        public ViewResult Post(int year, int month, string title)
        {
            var post = _blogRepository.Post(year, month, title);

            if (post == null)
                throw new HttpException(404, "Post not found");

            return View(post);
        }
    }
}