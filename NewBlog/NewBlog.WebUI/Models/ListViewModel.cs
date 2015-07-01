using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NewBlog.Domain.Entities;
using NewBlog.Domain.Abstract;

namespace NewBlog.WebUI.Models
{
    public class ListViewModel
    {
        public ListViewModel(IBlogRepository blogRepository, int p)
        {
            Posts = blogRepository.Posts(p - 1, 3).ToList();
            TotalPosts = blogRepository.TotalPosts();

            foreach (var post in Posts.Where(m => m.ImgUrl == null))
            {
                post.ImgUrl = "~/Content/Uploads/default.jpg";
            }            
        }

        public ListViewModel(IBlogRepository blogRepository,
        string text, string type, int p)
        {
            switch (type)
            {
                case "Category":
                    Posts = blogRepository.PostsForCategory(text, p - 1, 3).ToList();
                    TotalPosts = blogRepository.TotalPostsForCategory(text);
                    Category = blogRepository.Category(text);
                    break;
                case "Tag":
                    Posts = blogRepository.PostsForTag(text, p - 1, 3).ToList();
                    TotalPosts = blogRepository.TotalPostsForTag(text);
                    Tag = blogRepository.Tag(text);

                    break;
                default:
                    Posts = blogRepository.PostsForSearch(text, p - 1, 3).ToList();
                    TotalPosts = blogRepository.TotalPostsForSearch(text);
                    Search = text;
                    break;
            }
            foreach (var post in Posts.Where(m => m.ImgUrl == null))
            {
                post.ImgUrl = "~/Content/Uploads/default.jpg";
            }
        }

        public IList<Post> Posts { get; private set; }
        public int TotalPosts { get; private set; }
        public Category Category { get; private set; }
        public Tag Tag { get; private set; }
        public string Search { get; private set; }

    }
}