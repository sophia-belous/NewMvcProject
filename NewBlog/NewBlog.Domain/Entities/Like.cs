using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewBlog.Domain.Entities
{
    public class Like
    {
        [Key]
        public int LikeId { get; set; }
        public int PostId { get; set; }

        public virtual Post Post { get; set; }
    }
}
