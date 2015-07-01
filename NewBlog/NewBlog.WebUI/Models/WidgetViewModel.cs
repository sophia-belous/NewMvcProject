using NewBlog.Domain.Abstract;
using NewBlog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewBlog.WebUI.Models
{
    public class WidgetViewModel
    {
        public WidgetViewModel(IBlogRepository blogRepository)
        {
            Categories = blogRepository.Categories().ToList();
            Tags = blogRepository.Tags().ToList();
            LatestPosts = blogRepository.Posts(0, 5).ToList();
        }
 
        public IList<Category> Categories { get; private set; }
        public IList<Tag> Tags { get; private set; }
        public IList<Post> LatestPosts { get; private set; } 


    }
}