using BlogDemo.Core.Entities;
using BlogDemo.Core.Interface;
using BlogDemo.Infrastructure.Database;
using BlogDemo.Infrastructure.Extentions;
using BlogDemo.Infrastructure.Resources;
using BlogDemo.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogDemo.Infrastructure.Repository
{
    /// <summary>
    /// Repository that encapsulated database operations for Post object
    /// </summary>
    public class PostRepository: IPostRepository
    {
        private readonly MyContext _myContext;
        private readonly IPropertyMappingContainer _propertyMappingContainer;
        public PostRepository(MyContext myContext, 
            IPropertyMappingContainer propertyMappingContainer)
        {
            _myContext = myContext;
            _propertyMappingContainer = propertyMappingContainer;
        }
        
        /// <summary>
        /// Find an entity with a primary key value. 
        /// Return the entity if it is found otherwise null
        /// </summary>
        /// <param name="id">The value of the primary key for the entity</param>
        /// <returns>The entity or null</returns>
        public async Task<Post> GetPostByIdAsync(int id)
        {
            return await _myContext.Posts.FindAsync(id);
        }

        /// <summary>
        /// Notice: sorting before pagination
        /// </summary>
        /// <param name="postParameters">Constains</param>
        /// <returns>All the entity based on the constains</returns>
        public async Task<PaginatedList<Post>> GetAllPostsAsync(PostParameters postParameters)
        {
            var query = _myContext.Posts.AsQueryable();

            if (!string.IsNullOrEmpty(postParameters.Title))
            {
                var title = postParameters.Title.ToLowerInvariant();
                query = query.Where(x => x.Title.ToLowerInvariant().Contains(title));
            }

            query = query.ApplySort(postParameters.OrderBy, _propertyMappingContainer.Resolve<PostResource, Post>());

            var count = await query.CountAsync();

            var data = await query
                .Skip(postParameters.PageIndex * postParameters.PageSize)
                .Take(postParameters.PageSize)
                .ToListAsync();

            return new PaginatedList<Post>(postParameters.PageIndex, postParameters.PageSize, count, data);
        }

        public void AddPost(Post post)
        {
            _myContext.Add(post);
        }

        public void Delete(Post post)
        {
            _myContext.Remove(post); ;
        }

        public void Update(Post post)
        {
            _myContext.Entry(post).State = EntityState.Modified;
        }
    }
}
