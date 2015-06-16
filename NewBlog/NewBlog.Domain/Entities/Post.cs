using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace NewBlog.Domain.Entities
{
    public class Post
    {
        [Key]
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }

        [UIHint("tinymce_jquery_full"), AllowHtml]
        public string Description { get; set; }
        public string UrlSlug { get; set; }
        public string ImgUrl { get; set; }
        public string VdeoUrl { get; set; }
        public bool Published { get; set; }
        public DateTime PostedOn { get; set; }
        public DateTime? Modified { get; set; }

        public virtual Category Category { get; set; }
        public virtual IList<Tag> Tags { get; set; }
        public virtual IList<Comment> Comments { get; set; }
        public virtual IList<Like> Likes { get; set; }
    }
}
