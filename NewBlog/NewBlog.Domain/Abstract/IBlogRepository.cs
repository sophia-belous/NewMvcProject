using NewBlog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewBlog.Domain.Abstract
{
    public interface IBlogRepository : IDisposable
    {
        IQueryable<UserProfile> Users();
        
        IQueryable<Post> Posts(int pageNo, int pageSize);
        int TotalPosts();

        IQueryable<Post> PostsForCategory(string categorySlug, int pageNo, int pageSize);
        int TotalPostsForCategory(string categorySlug);
        Category Category(string categorySlug);

        IQueryable<Post> PostsForTag(string tagSlug, int pageNo, int pageSize);
        int TotalPostsForTag(string tagSlug);
        Tag Tag(string tagSlug);

        IQueryable<Post> PostsForSearch(string search, int pageNo, int pageSize);
        int TotalPostsForSearch(string search);

        Post Post(int id);

        IQueryable<Category> Categories();
        IQueryable<Tag> Tags();

        IQueryable<Post> Posts();
        
        void SavePost(Post post);
        Post DeletePost(int id);
        Tag GetFirstTag();
        Category GetFirstCategory();

        IQueryable<Comment> Comments();
        void SaveComment(Comment comment);

        void AddLike(int postId, string username);
        void RemoveLike(int postId, string username);

        void SaveCategory(Category category);
        void SaveTag(Tag tag);
        Category DeleteCategory(int id);
        Tag DeleteTag(int id);




    }
}
