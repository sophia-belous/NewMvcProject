using NewBlog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace NewBlog.WebUI
{
    public static class ActionLinkExtensions
    {
        public static MvcHtmlString PostLink(this HtmlHelper helper, Post post)
        {
            return helper.ActionLink(post.Title, "Post", "Blog", 
                new { 
                    id = post.Id
                },
                new { 
                    title = post.Title 
                });
        }
 
        public static MvcHtmlString CategoryLink(this HtmlHelper helper, 
            Category category)
        {
            return helper.ActionLink(category.Name, "Category", "Blog", 
                new { 
                    category = category.UrlSlug 
                }, 
                new { 
                    title = String.Format("See all posts in {0}", category.Name) 
                });
        }
     
        public static MvcHtmlString TagLink(this HtmlHelper helper, Tag tag)
        {
            return helper.ActionLink(tag.Name, "Tag", "Blog", new { tag = tag.UrlSlug},
                new
                {
                    title = String.Format("See all posts in {0}", tag.Name)
                });
        }

        public static IHtmlString Image(this HtmlHelper helper, string src)
        {
            TagBuilder tb = new TagBuilder("img");
            tb.Attributes.Add("src", VirtualPathUtility.ToAbsolute(src));
            return new MvcHtmlString(tb.ToString(TagRenderMode.SelfClosing));
        }

    }
}