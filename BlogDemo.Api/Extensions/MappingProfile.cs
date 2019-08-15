using AutoMapper;
using BlogDemo.Core.Entities;
using BlogDemo.Infrastructure.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogDemo.Api.Extensions
{
    /// <summary>
    /// Establish a mapping between Post and PostResource
    /// Post is used internal (Server/Database)
    /// PostResource is used external (Client)
    /// </summary>
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            // Mapping from Post to PostResource (Marked UpdateTime is equivalent to LastModified)
            CreateMap<Post,PostResource>()
                .ForMember(dest => dest.UpdateTime, opt => opt.MapFrom(src => src.LastModified));
            // Mapping from PsotResource to Post
            CreateMap<PostResource, Post>();

            CreateMap<PostAddResource, Post>();

            CreateMap<Post, PostUpdateResource>();
            CreateMap<PostUpdateResource, Post>();
        }
    }
}
