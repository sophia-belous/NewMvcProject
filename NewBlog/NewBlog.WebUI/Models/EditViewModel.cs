using NewBlog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NewBlog.WebUI.Models
{
    public class EditViewModel
    {
        public Post Post { get; set; }
        public MultiSelectList Tags { get; set; }
        public int[] TagIndexes { get; set; }
    }
}