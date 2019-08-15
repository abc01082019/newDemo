using System;
using System.Collections.Generic;
using System.Text;

namespace BlogDemo.Core.Entities
{
    /// <summary>
    /// Entity model whitch include meta data (Database data O/RM) used internal.
    /// </summary>
    public class Post: Entity
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public string Author { get; set; }
        public DateTime LastModified { get; set; }
        public string Remark { get; set; }
    }
}
