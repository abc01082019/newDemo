﻿using BlogDemo.Core.Entities;
using BlogDemo.Infrastructure.Database.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogDemo.Infrastructure.Database
{
    /// <summary>
    /// MyContext use to perform CRUD operations (Extended DbContext(EF)-class)
    /// </summary>
    public class MyContext: DbContext
    {
        public MyContext(DbContextOptions<MyContext> options)
            : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new PostConfiguration());
            modelBuilder.ApplyConfiguration(new PostImageConfiguration());
        }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostImage> PostImages { get; set; }
    }
}
