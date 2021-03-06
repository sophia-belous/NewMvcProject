﻿using Microsoft.Security.Application;
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
    public class BlogController : Controller
    {
        private readonly IBlogRepository _blogRepository;
        
        public BlogController(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        public ViewResult Posts(int p = 1)
        {
            var viewModel = new ListViewModel(_blogRepository, p);
            if (viewModel.Posts.Count > 0)
            {
                ViewBag.Title = "Latest Posts";
                return View("List", viewModel);
            }

            ViewBag.Message = "posts";
            ViewBag.Layout = "_Layout";
            return View("_NoElementsFound");
            
            
        }

        public ViewResult Post(int id)
        {
            CommentPostModel cpm = new CommentPostModel();
                cpm.Post = _blogRepository.Post(id);
                cpm.Comment = new Comment() { PostId = cpm.Post.Id };
                cpm.IsCommentsAllowed = User.Identity.IsAuthenticated? true : false;

            if (cpm.Post == null)
                throw new HttpException(404, "Post not found");

            if (cpm.Post.Published == false && User.Identity.IsAuthenticated == false)
                throw new HttpException(401, "The post is not published");

            return View(cpm);
        }

        [ValidateInput(false)]
        public ViewResult Search(string s, int p = 1)
        {
            s = Sanitizer.GetSafeHtmlFragment(s);
            ViewBag.Title = String.Format(@"Lists of posts found 
                        for search text ""{0}""", s);

            var viewModel = new ListViewModel(_blogRepository, s, "Search", p);
            return View("List", viewModel);
        }

        [ValidateInput(false)]
        public ViewResult Category(string category, int p = 1)
        {
            category = Sanitizer.GetSafeHtmlFragment(category);
            var viewModel = new ListViewModel(_blogRepository, category, "Category", p);

            if (viewModel.Category == null)
                throw new HttpException(404, "Category not found");

            ViewBag.Title = String.Format(@"Latest posts on category ""{0}""",
                viewModel.Category.Name);
            return View("List", viewModel);
        }

        [ValidateInput(false)]
        public ViewResult Tag(string tag, int p = 1)
        {
            tag = Sanitizer.GetSafeHtmlFragment(tag);
            var viewModel = new ListViewModel(_blogRepository, tag, "Tag", p);

            if (viewModel.Tag == null)
                throw new HttpException(404, "Tag not found");

            ViewBag.Title = String.Format(@"Latest posts tagged on ""{0}""",
                viewModel.Tag.Name);
            return View("List", viewModel);
        }

        [ChildActionOnly]
        public PartialViewResult Sidebars()
        {
            var widgetViewModel = new WidgetViewModel(_blogRepository);

            return PartialView("_Sidebars", widgetViewModel);
        }

        [Authorize]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [ChildActionOnly]
        public ActionResult AddComment(int? postId)
        {
            return View(new Comment() { PostId = (int)postId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult AddComment([Bind(Include = "CommentId,PostId,UserId,CommentedOn,Description")] Comment comment)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Post", new { id = comment.PostId });

            comment.Description = Sanitizer.GetSafeHtmlFragment(comment.Description);
            comment.CommentedOn = DateTime.Now;
            comment.User = _blogRepository.Users().First(x => x.Username == User.Identity.Name);

            _blogRepository.SaveComment(comment);

            return RedirectToAction("Post", new { id = comment.PostId });
            
        }

        [HttpPost]
        public ActionResult AddLike(int postId, bool state)
        {
            if (state)
                _blogRepository.AddLike(postId, User.Identity.Name);
            else
                _blogRepository.RemoveLike(postId, User.Identity.Name);

            return Json(new { success = true, likesCount = _blogRepository.Post(postId).Likes.Count },
                JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            _blogRepository.Dispose();
            base.Dispose(disposing);
        }

    }
}