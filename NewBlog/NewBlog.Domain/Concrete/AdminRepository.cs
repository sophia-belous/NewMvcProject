using NewBlog.Domain.Abstract;
using NewBlog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewBlog.Domain.Concrete
{
    public class AdminRepository : IAdminRepository
    {
        private readonly BlogDbContext _blogContext;

        public AdminRepository(BlogDbContext blogContext)
        {
            _blogContext = blogContext;
        }

        public IList<Post> Posts()
        {
            return _blogContext.Posts.ToList();
        }
    }
}
