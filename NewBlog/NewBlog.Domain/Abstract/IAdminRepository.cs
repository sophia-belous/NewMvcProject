using NewBlog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewBlog.Domain.Abstract
{
    public interface IAdminRepository
    {
        IList<Post> Posts();

    }
}
