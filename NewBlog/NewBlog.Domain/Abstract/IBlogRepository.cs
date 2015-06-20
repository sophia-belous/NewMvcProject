using NewBlog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewBlog.Domain.Abstract
{
    public interface IBlogRepository
    {
        IList<UserProfile> Users();
        
        IList<Post> Posts(int pageNo, int pageSize);
        int TotalPosts();

        IList<Post> PostsForCategory(string categorySlug, int pageNo, int pageSize);
        int TotalPostsForCategory(string categorySlug);
        Category Category(string categorySlug);

        IList<Post> PostsForTag(string tagSlug, int pageNo, int pageSize);
        int TotalPostsForTag(string tagSlug);
        Tag Tag(string tagSlug);

        IList<Post> PostsForSearch(string search, int pageNo, int pageSize);
        int TotalPostsForSearch(string search);

        Post Post(int id);

        IList<Category> Categories();
        IList<Tag> Tags();

        IList<Post> Posts();
        
        void SavePost(Post post);
        Post DeletePost(int id);
        Tag GetFirstTag();

        IList<Comment> Comments();
        void SaveComment(Comment comment);

        void AddLike(int postId, string username);
        void RemoveLike(int postId, string username);




    }
}
