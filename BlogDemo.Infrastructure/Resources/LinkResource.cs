using System;
using System.Collections.Generic;
using System.Text;

namespace BlogDemo.Infrastructure.Resources
{
    public class LinkResource
    {
        public LinkResource(string href, string rel, string method)
        {
            Href = href;
            Rel = rel;
            Method = method;
        }

        public string Href { get; }
        public string Rel { get; }
        public string Method { get; }
    }
}
