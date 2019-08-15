using BlogDemo.Core.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogDemo.Infrastructure.Services
{
    /// <summary>
    /// Establish a relation between the resource model and the entity model
    /// </summary>
    /// <typeparam name="ISource">The resource model</typeparam>
    /// <typeparam name="IDestination">The entity model</typeparam>
    public abstract class PropertyMapping<ISource, IDestination> : IPropertyMapping
        where IDestination : IEntity
    {
        public Dictionary<string, List<MappedProperty>> MappingDictionary { get; }

        protected PropertyMapping(Dictionary<string, List<MappedProperty>> mappingDictionary)
        {
            MappingDictionary = mappingDictionary;
            MappingDictionary[nameof(IEntity.Id)] = new List<MappedProperty>
            {
                new MappedProperty { Name = nameof(IEntity.Id), Reverse = false }
            };
        }
    }
}
