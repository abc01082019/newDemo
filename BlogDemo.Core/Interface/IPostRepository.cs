using BlogDemo.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlogDemo.Core.Interface
{
    /// <summary>
    /// Interface for the Post features
    /// </summary>
    public interface IPostRepository
    {
        Task<Post> GetPostByIdAsync(int id);
        Task<PaginatedList<Post>> GetAllPostsAsync(PostParameters postParameters);
        void AddPost(Post post);
        void Delete(Post post);
        void Update(Post post);
    }
}
