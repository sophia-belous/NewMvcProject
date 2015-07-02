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
            NotNullOfCategories = Categories.Count > 0 ? true : false; 

            Tags = blogRepository.Tags().ToList();
            NotNullOfTags = Tags.Count > 0 ? true : false;

            LatestPosts = blogRepository.Posts(0, 5).ToList();
            NotNullOfPosts = LatestPosts.Count > 0 ? true : false;
        }
 
        public IList<Category> Categories { get; private set; }
        public IList<Tag> Tags { get; private set; }
        public IList<Post> LatestPosts { get; private set; }
        public bool NotNullOfCategories { get; private set; }
        public bool NotNullOfTags { get; private set; }
        public bool NotNullOfPosts { get; private set; }


    }
}