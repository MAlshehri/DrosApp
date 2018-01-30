using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Dros.Data.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace DrosDbCreator
{
    public class DrosDbContext : DbContext
    {
        public DbSet<Material> Materials { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Link> Links { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=dros.db");
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<MaterialAuthor>()
                .HasKey(t => new { t.MaterialId, t.AuthorId });

            modelBuilder.Entity<MaterialTag>()
                .HasKey(t => new { t.MaterialId, t.TagId });

            modelBuilder.Entity<MaterialCategory>()
                .HasKey(t => new { t.MaterialId, t.CategoryId });

            modelBuilder.Entity<MaterialAuthor>()
                .HasOne(pt => pt.Material)
                .WithMany(p => p.Authors)
                .HasForeignKey(pt => pt.MaterialId);

            modelBuilder.Entity<MaterialAuthor>()
                .HasOne(pt => pt.Author)
                .WithMany(t => t.Materials)
                .HasForeignKey(pt => pt.AuthorId);

            modelBuilder.Entity<MaterialTag>()
                .HasOne(pt => pt.Material)
                .WithMany(p => p.Tags)
                .HasForeignKey(pt => pt.MaterialId);

            modelBuilder.Entity<MaterialTag>()
                .HasOne(pt => pt.Tag)
                .WithMany(t => t.Materials)
                .HasForeignKey(pt => pt.TagId);

            modelBuilder.Entity<MaterialCategory>()
                .HasOne(pt => pt.Material)
                .WithMany(p => p.Categories)
                .HasForeignKey(pt => pt.MaterialId);

            modelBuilder.Entity<MaterialCategory>()
                .HasOne(pt => pt.Category)
                .WithMany(t => t.Materials)
                .HasForeignKey(pt => pt.CategoryId);


            base.OnModelCreating(modelBuilder);
        }
    }
}
