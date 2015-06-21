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

           /* _blogContext.Tags.Add(new Tag() { Name = "Cool", UrlSlug = "Cool" });
            _blogContext.Tags.Add(new Tag() { Name = "Interesting", UrlSlug = "Interesting" });
            _blogContext.Tags.Add(new Tag() { Name = "ILike", UrlSlug = "ILike" });
            _blogContext.Tags.Add(new Tag() { Name = "Buetifull", UrlSlug = "Buetifull" });
            _blogContext.Tags.Add(new Tag() { Name = "SomeTag", UrlSlug = "SomeTag" });
            _blogContext.Tags.Add(new Tag() { Name = "NewTag", UrlSlug = "NewTag" });

            _blogContext.SaveChanges();


            Category cat = _blogContext.Categories.First();
            Tag tag = _blogContext.Tags.First();
            List<Tag> tags = new List<Tag>() { tag };


            _blogContext.Posts.Add(new Post() { Title = "elevenPost", 
                ShortDescription = "ShortDescription 11", 
                Description = "Description 11", 
                Published = true, 
                PostedOn = DateTime.Now, 
                Category = cat,
                CategoryId = cat.CategoryId, Tags = tags});

            _blogContext.SaveChanges();*/
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


        public IList<Post> PostsForCategory(string categorySlug, int pageNo, int pageSize)
        {
            var posts = _blogContext.Posts
                      .Where(p => p.Published && p.Category.UrlSlug.Equals(categorySlug))
                      .OrderByDescending(p => p.PostedOn)
                      .Skip(pageNo * pageSize)
                      .Take(pageSize)
                      .Include(p => p.Category)
                      .ToList();

            var postIds = posts.Select(p => p.Id).ToList();

            return _blogContext.Posts
                .Where(p => postIds.Contains(p.Id))
                .OrderByDescending(p => p.PostedOn)
                .Include(p => p.Tags)
                .ToList();
        }

        public int TotalPostsForCategory(string categorySlug)
        {
            return _blogContext.Posts
                .Where(p => p.Published && p.Category.UrlSlug.Equals(categorySlug))
                .Count();
        }

        public Category Category(string categorySlug)
        {
            return _blogContext.Categories
                .FirstOrDefault(t => t.UrlSlug.Equals(categorySlug));

        }


        public IList<Post> PostsForTag(string tagSlug, int pageNo, int pageSize)
        {
            var posts = _blogContext.Posts
                    .Where(p => p.Published && p.Tags.Any(t => t.UrlSlug.Equals(tagSlug)))
                    .OrderByDescending(p => p.PostedOn)
                    .Skip(pageNo * pageSize)
                    .Take(pageSize)
                    .Include(p => p.Category)
                    .ToList();

            var postIds = posts.Select(p => p.Id).ToList();

            return _blogContext.Posts
                          .Where(p => postIds.Contains(p.Id))
                          .OrderByDescending(p => p.PostedOn)
                          .Include(p => p.Tags)
                          .ToList();

        }

        public int TotalPostsForTag(string tagSlug)
        {
            return _blogContext.Posts
                .Where(p => p.Published && p.Tags.Any(t => t.UrlSlug.Equals(tagSlug)))
                .Count();
        }

        public Tag Tag(string tagSlug)
        {
            return _blogContext.Tags
                .FirstOrDefault(t => t.UrlSlug.Equals(tagSlug));
        }


        public IList<Post> PostsForSearch(string search, int pageNo, int pageSize)
        {
            var posts = _blogContext.Posts
                        .Where(p => p.Published && (p.Title.Contains(search) || p.Category.Name.Equals(search) || p.Tags.Any(t => t.Name.Equals(search))))
                        .OrderByDescending(p => p.PostedOn)
                        .Skip(pageNo * pageSize)
                        .Take(pageSize)
                        .Include(p => p.Category)
                        .ToList();

            var postIds = posts.Select(p => p.Id).ToList();

            return _blogContext.Posts
                  .Where(p => postIds.Contains(p.Id))
                  .OrderByDescending(p => p.PostedOn)
                  .Include(p => p.Tags)
                  .ToList();
        }

        public int TotalPostsForSearch(string search)
        {
            return _blogContext.Posts
            .Where(p => p.Published && (p.Title.Contains(search) || p.Category.Name.Equals(search) || p.Tags.Any(t => t.Name.Equals(search))))
            .Count();

        }

        public Post Post(int id)
        {
            //var query = _blogContext.Posts.Where(p => p.UrlSlug.Equals(titleSlug))
            //            .Include(p => p.Category);

            //query.Include(p => p.Tags);

            return _blogContext.Posts.Single(p => p.Id == id);

        }

        public IList<Category> Categories()
        {
            return _blogContext.Categories.OrderBy(p => p.Name).ToList();
        }


        public IList<Tag> Tags()
        {
            return _blogContext.Tags.OrderBy(p => p.Name).ToList();
        }

        public IList<Post> Posts()
        {
            return _blogContext.Posts.ToList();
        }


        public void SavePost(Post post)
        {
            if (post.Id == 0)
            {
                post.PostedOn = DateTime.Now;
                _blogContext.Posts.Add(post);                
            }
            else
            {
                Post postFind = _blogContext.Posts.Include("Tags").Single(p => p.Id == post.Id);

                if (postFind != null)
                {
                    postFind.Title = post.Title;
                    postFind.ShortDescription = post.ShortDescription;
                    postFind.Description = post.Description;
                    postFind.Category = post.Category;
                    postFind.ImgUrl = post.ImgUrl;
                    postFind.Modified = DateTime.Now;
                    postFind.Published = post.Published;
                    postFind.Tags = post.Tags.ToList();
                    postFind.UrlSlug = post.UrlSlug;
                }
            }

            _blogContext.SaveChanges();
        }

        public Tag GetFirstTag()
        {
            return _blogContext.Tags.First();
        }


        public Post DeletePost(int id)
        {
            Post dbEntry = _blogContext.Posts.Find(id);
            if (dbEntry != null)
            {
                _blogContext.Posts.Remove(dbEntry);
                _blogContext.SaveChanges();
            }
            return dbEntry;
        }

        public IList<UserProfile> Users()
        {
            return _blogContext.Users.ToList();
        }


        public IList<Comment> Comments()
        {
            return _blogContext.Comments.ToList();
        }

        public void SaveComment(Comment comment)
        {
            comment.CommentedOn = DateTime.Now;
            _blogContext.Comments.Add(comment);

            _blogContext.SaveChanges();
        }


        public void AddLike(int postId, string username)
        {
            var user = _blogContext.Users.First(u => u.Username == username);
            user.Likes.Add(new Like
            {
                PostId = postId
            });
            _blogContext.SaveChanges();
        }

        public void RemoveLike(int postId, string username)
        {
            var like = _blogContext.Likes.Include("User").First(l => (l.PostId == postId && l.User.Username == username));
            _blogContext.Likes.Remove(like);
            _blogContext.SaveChanges();
        }
    }
}
