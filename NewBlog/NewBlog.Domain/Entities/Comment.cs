using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewBlog.Domain.Entities
{
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }
        public int PostId { get; set; }
        public int UserId { get; set; }

        [Required]
        public DateTime CommentedOn { get; set; }

        [Required(ErrorMessage = "Please, write your comment!")]
        public string Description { get; set; }

        public virtual Post Post { get; set; }
        [ForeignKey("UserId")]
        public virtual UserProfile User { get; set; }
    }
}
