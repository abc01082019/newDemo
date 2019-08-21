using BlogDemo.Core.Entities;
using BlogDemo.Core.Interface;
using BlogDemo.Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogDemo.Infrastructure.Repository
{
    /// <summary>
    /// Repository that encapsulated CURD operations for PostImage object
    /// </summary>
    public class PostImageRepository: IPostImageRepository
    {
        private readonly MyContext _myContext;

        public PostImageRepository(MyContext myContex)
        {
            _myContext = myContex;
        }

        public void Add(PostImage postImage)
        {
            _myContext.PostImages.Add(postImage);
        }
    }
}
