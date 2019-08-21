using System;
using System.Collections.Generic;
using System.Text;

namespace BlogDemo.Infrastructure.Resources
{
    /// <summary>
    /// DTO: domain transfer object that map to domain object
    /// </summary>
    /// 
    public class PostImageResource
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string Location => $"/{PostImagePath.Folder}/{FileName}";
    }
}
