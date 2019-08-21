using BlogDemo.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogDemo.Infrastructure.Database.EntityConfigurations
{
    /// <summary>
    /// Set validations/constrains for the data-resources/domain model
    /// </summary>
    public class PostConfiguration: IEntityTypeConfiguration<Post>
    {
        // Set constrains to Post data-model (sqlte EF don't support you to alter columns by Migration)
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            // (sqlte EF don't support you to alter columns by Migration)
            //builder.Property(x => x.Author).IsRequired().HasMaxLength(50);
            //builder.Property(x => x.Title).IsRequired().HasMaxLength(100);
            //builder.Property(x => x.Body).IsRequired().HasColumnType("nvarchar(max)");
            builder.Property(x => x.Remark).HasMaxLength(200);

        }
    }
}
