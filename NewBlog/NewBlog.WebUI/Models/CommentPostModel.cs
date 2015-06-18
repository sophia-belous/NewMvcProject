using NewBlog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewBlog.WebUI.Models
{
    public class CommentPostModel
    {
        public Post Post { get; set; }
        public Comment Comment { get; set; }
    }
}