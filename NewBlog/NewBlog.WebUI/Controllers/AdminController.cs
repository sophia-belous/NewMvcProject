using Microsoft.Security.Application;
using NewBlog.Domain.Abstract;
using NewBlog.Domain.Entities;
using NewBlog.WebUI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace NewBlog.WebUI.Controllers
{
    [Authorize(Roles="Admin")]
    public class AdminController : Controller
    {
        IBlogRepository _blogRepository;

        public AdminController(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        public ViewResult List()
        {
            if (_blogRepository.Posts().ToList().Count > 0)
               return View(_blogRepository.Posts().ToList());

            return View("<p>No posts found!</p>");
        }

        [HttpGet]
        public ActionResult Create()
        {
            Tag tag = _blogRepository.GetFirstTag();
            List<Tag> tags = new List<Tag>() { tag };

            Category category = _blogRepository.GetFirstCategory();
            List<Category> categories = new List<Category> { category};

            Post post = new Post() { Tags = tags, Category = category};
            IList<Tag> allTags = _blogRepository.Tags().ToList();
            IList<Category> allCategories = _blogRepository.Categories().ToList();
            int[] tagIndexes = post.Tags.Select(x => x.TagId).ToArray();
            int[] categoryIndexes = new int[] { category.CategoryId };            

            EditViewModel evm = new EditViewModel();
            evm.Post = post;
            evm.Tags = new MultiSelectList(allTags, "TagId", "Name", tagIndexes);
            evm.Categories = new MultiSelectList(allCategories, "CategoryId", "Name", categoryIndexes);

            return View("Edit", evm);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(EditViewModel evm, HttpPostedFileBase file)
        {
            if (!ModelState.IsValid)
                return View(evm.Post);

            evm.Post.Description = Sanitizer.GetSafeHtmlFragment(evm.Post.Description);
            evm.Post.ShortDescription = Sanitizer.GetSafeHtmlFragment(evm.Post.ShortDescription);
            evm.Post.Title = Sanitizer.GetSafeHtmlFragment(evm.Post.Title);
            evm.Post.VdeoUrl = Sanitizer.GetSafeHtmlFragment(evm.Post.VdeoUrl);            

            if (file != null)
            {
                string path = Server.MapPath("~/Content/Uploads/" + file.FileName);
                file.SaveAs(path);
                evm.Post.ImgUrl = "~/Content/Uploads/" + file.FileName;
            }                    

            List<Tag> tags = new List<Tag>();
            IList<Tag> allTags = _blogRepository.Tags().ToList();

            for (int i = 0; i < evm.TagIndexes.Length; i++)
                tags.Add(allTags.First(t => t.TagId == evm.TagIndexes[i]));

            List<Category> categories = new List<Category>();
            IList<Category> allCategories = _blogRepository.Categories().ToList();
            categories.Add(allCategories.First(c => c.CategoryId == evm.CategoryIndexes[0]));

            evm.Post.Tags = tags;
            evm.Post.Category = categories.FirstOrDefault();

            _blogRepository.SavePost(evm.Post);
            TempData["message"] = string.Format(" Post \"{0}\" was created", evm.Post.Title);

            return RedirectToAction("List");                
        }


        [HttpGet]
        public ViewResult Edit(int id)
        {
            Post post = _blogRepository.Posts()
                .FirstOrDefault(g => g.Id == id);

            IList<Tag> allTags = _blogRepository.Tags().ToList();
            int[] tagIndexes = post.Tags.Select(x => x.TagId).ToArray();

            IList<Category> allCategories = _blogRepository.Categories().ToList();
            int[] categoryIndexes = new int[] { post.Category.CategoryId };

            EditViewModel evm = new EditViewModel();
            evm.Post = post;
            evm.Tags = new MultiSelectList(allTags, "TagId", "Name", tagIndexes);
            evm.Categories = new MultiSelectList(allCategories, "CategoryId", "Name", categoryIndexes);

            return View(evm);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(EditViewModel evm, HttpPostedFileBase file)
        {
            evm.Post.Description = Sanitizer.GetSafeHtmlFragment(evm.Post.Description);
            evm.Post.ShortDescription = Sanitizer.GetSafeHtmlFragment(evm.Post.ShortDescription);
            evm.Post.Title = Sanitizer.GetSafeHtmlFragment(evm.Post.Title);
            evm.Post.VdeoUrl = Sanitizer.GetSafeHtmlFragment(evm.Post.VdeoUrl);

            if (!ModelState.IsValid)
                return View(evm.Post);

            if (file == null && _blogRepository.Post(evm.Post.Id).ImgUrl != null)
                evm.Post.ImgUrl = _blogRepository.Post(evm.Post.Id).ImgUrl;                    
            else if (file != null)
            {
                if (_blogRepository.Post(evm.Post.Id).ImgUrl != null)
                {
                    string fullPath = Request.MapPath("~/Content/Uploads/" + _blogRepository.Post(evm.Post.Id).ImgUrl);

                    if (System.IO.File.Exists(fullPath))
                        System.IO.File.Delete(fullPath);
                }                    

                string path = Server.MapPath("~/Content/Uploads/" + file.FileName);
                                file.SaveAs(path);
                evm.Post.ImgUrl = "~/Content/Uploads/" + file.FileName;                                  
            }               

            List<Tag> tags = new List<Tag>();
            IList<Tag> allTags = _blogRepository.Tags().ToList();
            for (int i = 0; i < evm.TagIndexes.Length; i++)
                tags.Add(allTags.First(t => t.TagId == evm.TagIndexes[i]));

            List<Category> categories = new List<Category>();
            IList<Category> allCategories = _blogRepository.Categories().ToList();
            categories.Add(allCategories.First(c => c.CategoryId == evm.CategoryIndexes[0]));

            evm.Post.Tags = tags;
            evm.Post.Category = categories.FirstOrDefault();

            _blogRepository.SavePost(evm.Post);
            TempData["message"] = string.Format("Changes in Post \"{0}\" were saved", evm.Post.Title);

            return RedirectToAction("List");
                
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            Post deletedPost = _blogRepository.DeletePost(id);

            if (deletedPost != null)
                TempData["message"] = string.Format("Post \"{0}\" was deleted", deletedPost.Title);

            return RedirectToAction("List");
        }

        public ActionResult Categories()
        {
            if (_blogRepository.Categories().ToList().Count > 0)
                return View(_blogRepository.Categories());

            ViewBag.Message = "categories";
            ViewBag.Action = "AddCategory";
            ViewBag.Layout = "_AdminLayout";
            return View("_NoElementsFound");
            
        }

        public ActionResult Tags()
        {
            if (_blogRepository.Tags().ToList().Count > 0)
                return View(_blogRepository.Tags());

            ViewBag.Message = "tags";
            ViewBag.Action = "AddTag";
            ViewBag.Layout = "_AdminLayout";
            return View("_NoElementsFound");
        }


        [ChildActionOnly]
        public ActionResult AddCategory()
        {
            return View(new Category());
        }

        
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult AddCategory([Bind(Include = "CategoryId, Name")]Category category)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Categories");

            category.Name = Sanitizer.GetSafeHtmlFragment(category.Name);
            _blogRepository.SaveCategory(category);

            return RedirectToAction("Categories");
            
            
        }

        [ChildActionOnly]
        public ActionResult AddTag()
        {
            return View(new Tag());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult AddTag([Bind(Include = "TagId, Name")]Tag tag)
        {
            tag.Name = Sanitizer.GetSafeHtmlFragment(tag.Name);

            if (!ModelState.IsValid)
                return RedirectToAction("Tags");

                _blogRepository.SaveTag(tag);
                return RedirectToAction("Tags");
            

        }

        [HttpPost]
        public ActionResult DeleteCategory(int id)
        {
            Category deletedCategory = _blogRepository.DeleteCategory(id);

            if (deletedCategory != null)
                TempData["message"] = string.Format("Category \"{0}\" was deleted", deletedCategory.Name);

            return RedirectToAction("Categories");
        }

        [HttpPost]
        public ActionResult DeleteTag(int id)
        {
            Tag deletedTag = _blogRepository.DeleteTag(id);

            if (deletedTag != null)
                TempData["message"] = string.Format("Tag \"{0}\" was deleted", deletedTag.Name);

            return RedirectToAction("Tags");
        }

        public ActionResult Logout()
        {
            WebSecurity.Logout();
            return RedirectToAction("Posts", "Blog");
        }

        protected override void Dispose(bool disposing)
        {
            _blogRepository.Dispose();
            base.Dispose(disposing);
        }
    }
}