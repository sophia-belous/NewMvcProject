using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        [Required(ErrorMessage = "Please, enter title of post")]
        public string Title { get; set; }

        [DisplayName("Description")]
        [Required(ErrorMessage = "Please, enter short description of post")]
        public string ShortDescription { get; set; }

        [DisplayName("Content")]
        [Required(ErrorMessage = "Please, enter description of post")]
        public string Description { get; set; }
        public string UrlSlug { get; set; }

        [DisplayName("Load Image")]
        public string ImgUrl { get; set; }

        [DisplayName("Video URL")]
        public string VdeoUrl { get; set; }
        public bool Published { get; set; }

        [DisplayName("Posted on")]
        [Required(ErrorMessage = "PostedOn: Field is required")]
        public DateTime PostedOn { get; set; }
        public DateTime? Modified { get; set; }

        public virtual Category Category { get; set; }
        public virtual IList<Tag> Tags { get; set; }
        public virtual IList<Comment> Comments { get; set; }
        public virtual IList<Like> Likes { get; set; }
    }
}
