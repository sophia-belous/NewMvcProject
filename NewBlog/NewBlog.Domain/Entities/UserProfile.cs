using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewBlog.Domain.Entities
{
    [Table("UserProfile")]
    public class UserProfile
    {
        [Key]
        public int UserId { get; set; }
        public string Username { get; set; }

        public virtual IList<Like> Likes { get; set; }
        public virtual IList<Comment> Comments { get; set; }
    }
}
