using BlogDemo.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogDemo.Core.Interface
{
    public interface IPostImageRepository
    {
        void Add(PostImage postImage);
    }
}
