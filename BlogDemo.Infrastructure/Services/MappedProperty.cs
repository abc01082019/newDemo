using System;
using System.Collections.Generic;
using System.Text;

namespace BlogDemo.Infrastructure.Services
{
    /// <summary>
    /// Include an entity name and 
    /// a flag variable of the mapping relation between entity and resource is reverse or not
    /// </summary>
    public class MappedProperty
    {
        public string Name { get; set; }
        public bool Reverse { get; set; }
    }
}
