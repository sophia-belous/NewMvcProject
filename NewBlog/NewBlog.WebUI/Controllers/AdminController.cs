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
            Post post = new Post() { Tags = tags };
            IList<Tag> allTags = _blogRepository.Tags();
            int[] tagIndexes = post.Tags.Select(x => x.TagId).ToArray();

            EditViewModel evm = new EditViewModel();
            evm.Post = post;
            evm.Tags = new MultiSelectList(allTags, "TagId", "Name", tagIndexes);

            return View(evm);
        }

        [HttpPost]
        public ActionResult Create(EditViewModel evm)
        {
            if (ModelState.IsValid)
            {
                List<Tag> tags = new List<Tag>();
                IList<Tag> allTags = _blogRepository.Tags();
                for (int i = 0; i < evm.TagIndexes.Length; i++)
                {
                    tags.Add(allTags.First(t => t.TagId == evm.TagIndexes[i]));
                }
                evm.Post.Tags = tags;

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

            IList<Tag> allTags = _blogRepository.Tags();

            int[] tagIndexes = post.Tags.Select(x => x.TagId).ToArray();

            EditViewModel evm = new EditViewModel();
            evm.Post = post;
            evm.Tags = new MultiSelectList(allTags, "TagId", "Name", tagIndexes);

            return View(evm);
        }

        [HttpPost]
        public ActionResult Edit(EditViewModel evm, HttpPostedFileBase file)
        {

                if (ModelState.IsValid)
            {
                string path = Server.MapPath("~/Content/Uploads/" + file.FileName);
                file.SaveAs(path);
                evm.Post.ImgUrl = "~/Content/Uploads/" + file.FileName;

                List<Tag> tags = new List<Tag>();
                IList<Tag> allTags = _blogRepository.Tags();
                for (int i = 0; i < evm.TagIndexes.Length; i++)
                {
                    tags.Add(allTags.First(t => t.TagId == evm.TagIndexes[i]));
                }
                evm.Post.Tags = tags;

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCategory([Bind(Include = "CategoryId, Name")]Category category)
        {
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
        public ActionResult AddTag([Bind(Include = "TagId, Name")]Tag tag)
        {
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