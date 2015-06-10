using NewBlog.Domain.Abstract;
using NewBlog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace NewBlog.Domain.Concrete
{
    public class BlogRepository : IBlogRepository
    {
        private readonly BlogDbContext _blogContext;

        public BlogRepository(BlogDbContext blogContext)
        {
            _blogContext = blogContext;
        }

        public IList<Post> Posts(int pageNo, int pageSize)
        {
            var query = _blogContext.Posts
                .Where(p => p.Published)
                .OrderByDescending(p => p.PostedOn)
                .Skip(pageNo * pageSize)
                .Take(pageSize)
                .Include(b => b.Category).ToList();

            return query;

        }

        public int TotalPosts()
        {
            return _blogContext.Posts.Where(p => p.Published).Count();
        }

    }
}
