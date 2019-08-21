using BlogDemo.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogDemo.Infrastructure.Database.EntityConfigurations
{
    /// <summary>
    /// Set validations/constains for the image values/domain model
    /// </summary>
    public class PostImageConfiguration : IEntityTypeConfiguration<PostImage>
    {
        public void Configure(EntityTypeBuilder<PostImage> builder)
        {
            builder.Property(x => x.FileName)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
