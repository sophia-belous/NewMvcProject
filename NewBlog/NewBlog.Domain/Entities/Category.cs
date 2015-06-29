using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewBlog.Domain.Entities
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [DisplayName("Category")]
        [Required(ErrorMessage = "Name: Field is required")]
        [StringLength(500, ErrorMessage = "Length should not exceed 500 characters")]
        public string Name { get; set; }
        public string UrlSlug { get; set; }
        public string Description { get; set; }

        public virtual IList<Post> Posts { get; set; }
    }
}
