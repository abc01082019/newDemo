﻿using System;
using System.Collections.Generic;
using System.Text;

namespace BlogDemo.Infrastructure.Resources
{
    /// <summary>
    /// PostResource model is used External (Web/UI) class, that map to the Post class
    /// </summary>
    public class PostResource
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string Author { get; set; }
        public DateTime UpdateTime { get; set; }
        public string Remark { get; set; }
    }
}
