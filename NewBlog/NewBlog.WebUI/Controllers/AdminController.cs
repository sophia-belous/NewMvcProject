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
            return View(_blogRepository.Posts());
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

            return View(evm);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(EditViewModel evm, HttpPostedFileBase file)
        {
            evm.Post.Description = Sanitizer.GetSafeHtmlFragment(evm.Post.Description);
            evm.Post.ShortDescription = Sanitizer.GetSafeHtmlFragment(evm.Post.ShortDescription);
            evm.Post.Title = Sanitizer.GetSafeHtmlFragment(evm.Post.Title);
            evm.Post.VdeoUrl = Sanitizer.GetSafeHtmlFragment(evm.Post.VdeoUrl);

            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    string path = Server.MapPath("~/Content/Uploads/" + file.FileName);
                    file.SaveAs(path);
                    evm.Post.ImgUrl = "~/Content/Uploads/" + file.FileName;
                }
                    

                List<Tag> tags = new List<Tag>();
                IList<Tag> allTags = _blogRepository.Tags().ToList();
                for (int i = 0; i < evm.TagIndexes.Length; i++)
                {
                    tags.Add(allTags.First(t => t.TagId == evm.TagIndexes[i]));
                }

                List<Category> categories = new List<Category>();
                IList<Category> allCategories = _blogRepository.Categories().ToList();
                categories.Add(allCategories.First(c => c.CategoryId == evm.CategoryIndexes[0]));

                evm.Post.Tags = tags;
                evm.Post.Category = categories.FirstOrDefault();

                _blogRepository.SavePost(evm.Post);
                TempData["message"] = string.Format(" Post \"{0}\" was created", evm.Post.Title);
                return RedirectToAction("List");
            }
            else
            {
                return View(evm.Post);
            }

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
            
            if (ModelState.IsValid)
            {

                if (file == null && _blogRepository.Post(evm.Post.Id).ImgUrl != null)
                {
                    evm.Post.ImgUrl = _blogRepository.Post(evm.Post.Id).ImgUrl;                    
                }
                else if (file != null)
                {
                    if (_blogRepository.Post(evm.Post.Id).ImgUrl != null)
                    {
                        string fullPath = Request.MapPath("~/Content/Uploads/" + _blogRepository.Post(evm.Post.Id).ImgUrl);

                        if (System.IO.File.Exists(fullPath))
                        {
                            System.IO.File.Delete(fullPath);
                        }
                    }                    

                    string path = Server.MapPath("~/Content/Uploads/" + file.FileName);
                                    file.SaveAs(path);
                    evm.Post.ImgUrl = "~/Content/Uploads/" + file.FileName;                                  
                }               

                List<Tag> tags = new List<Tag>();
                IList<Tag> allTags = _blogRepository.Tags().ToList();
                for (int i = 0; i < evm.TagIndexes.Length; i++)
                {
                    tags.Add(allTags.First(t => t.TagId == evm.TagIndexes[i]));
                }

                List<Category> categories = new List<Category>();
                IList<Category> allCategories = _blogRepository.Categories().ToList();
                categories.Add(allCategories.First(c => c.CategoryId == evm.CategoryIndexes[0]));

                evm.Post.Tags = tags;
                evm.Post.Category = categories.FirstOrDefault();

                _blogRepository.SavePost(evm.Post);
                TempData["message"] = string.Format("Changes in Post \"{0}\" were saved", evm.Post.Title);
                return RedirectToAction("List");
            }
            else
            {
                return View(evm.Post);
            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            Post deletedPost = _blogRepository.DeletePost(id);
            if (deletedPost != null)
            {
                TempData["message"] = string.Format("Post \"{0}\" was deleted",
                    deletedPost.Title);
            }
            return RedirectToAction("List");
        }

        public ActionResult Categories()
        {
            return View(_blogRepository.Categories());
        }

        public ActionResult Tags()
        {
            return View(_blogRepository.Tags());
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
            category.Name = Sanitizer.GetSafeHtmlFragment(category.Name);

            if (ModelState.IsValid)
            {
               _blogRepository.SaveCategory(category);
                return RedirectToAction("Categories");
            }
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
            if (ModelState.IsValid)
            {
                _blogRepository.SaveTag(tag);
                return RedirectToAction("Tags");
            }
            return RedirectToAction("Tags");

        }

        [HttpPost]
        public ActionResult DeleteCategory(int id)
        {
            Category deletedCategory = _blogRepository.DeleteCategory(id);
            if (deletedCategory != null)
            {
                TempData["message"] = string.Format("Category \"{0}\" was deleted",
                    deletedCategory.Name);
            }
            return RedirectToAction("Categories");
        }

        [HttpPost]
        public ActionResult DeleteTag(int id)
        {
            Tag deletedTag = _blogRepository.DeleteTag(id);
            if (deletedTag != null)
            {
                TempData["message"] = string.Format("Tag \"{0}\" was deleted",
                    deletedTag.Name);
            }
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