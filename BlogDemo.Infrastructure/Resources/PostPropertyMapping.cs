using BlogDemo.Core.Entities;
using BlogDemo.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogDemo.Infrastructure.Resources
{
    public class PostPropertyMapping : PropertyMapping<PostResource, Post>
    {
        public PostPropertyMapping() : base(
            new Dictionary<string, List<MappedProperty>> (StringComparer.OrdinalIgnoreCase)
            {
                // Build the mapping relation from resource to entity
                [nameof(PostResource.Title)] = new List<MappedProperty>
                {
                    new MappedProperty{ Name = nameof(Post.Title), Reverse = false}
                },
                [nameof(PostResource.Body)] = new List<MappedProperty>
                {
                    new MappedProperty{ Name = nameof(Post.Body), Reverse = false}
                },
                [nameof(PostResource.Author)] = new List<MappedProperty>
                {
                    new MappedProperty{ Name = nameof(Post.Author), Reverse = false}
                },
            })
        {

        }
    }
}
