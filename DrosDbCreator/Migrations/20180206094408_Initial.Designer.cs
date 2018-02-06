﻿// <auto-generated />
using Dros.Data.Models;
using DrosDbCreator;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace DrosDbCreator.Migrations
{
    [DbContext(typeof(DrosDbContext))]
    [Migration("20180206094408_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125");

            modelBuilder.Entity("Dros.Data.Models.Author", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTimeOffset>("CreatedOn");

                    b.Property<string>("Name");

                    b.Property<DateTimeOffset>("UpdatedOn");

                    b.HasKey("Id");

                    b.ToTable("Authors");
                });

            modelBuilder.Entity("Dros.Data.Models.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTimeOffset>("CreatedOn");

                    b.Property<string>("Name");

                    b.Property<DateTimeOffset>("UpdatedOn");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Dros.Data.Models.Link", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AudioType");

                    b.Property<string>("Content");

                    b.Property<long>("ContentLength");

                    b.Property<DateTimeOffset>("CreatedOn");

                    b.Property<Guid>("MaterialId");

                    b.Property<int>("Order");

                    b.Property<DateTimeOffset>("UpdatedOn");

                    b.HasKey("Id");

                    b.HasIndex("MaterialId");

                    b.ToTable("Links");
                });

            modelBuilder.Entity("Dros.Data.Models.Material", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Body");

                    b.Property<DateTimeOffset>("CreatedOn");

                    b.Property<string>("Language");

                    b.Property<string>("Title");

                    b.Property<DateTimeOffset>("UpdatedOn");

                    b.HasKey("Id");

                    b.ToTable("Materials");
                });

            modelBuilder.Entity("Dros.Data.Models.MaterialAuthor", b =>
                {
                    b.Property<Guid>("MaterialId");

                    b.Property<Guid>("AuthorId");

                    b.HasKey("MaterialId", "AuthorId");

                    b.HasIndex("AuthorId");

                    b.ToTable("MaterialAuthor");
                });

            modelBuilder.Entity("Dros.Data.Models.MaterialCategory", b =>
                {
                    b.Property<Guid>("MaterialId");

                    b.Property<Guid>("CategoryId");

                    b.HasKey("MaterialId", "CategoryId");

                    b.HasIndex("CategoryId");

                    b.ToTable("MaterialCategory");
                });

            modelBuilder.Entity("Dros.Data.Models.MaterialTag", b =>
                {
                    b.Property<Guid>("MaterialId");

                    b.Property<Guid>("TagId");

                    b.HasKey("MaterialId", "TagId");

                    b.HasIndex("TagId");

                    b.ToTable("MaterialTag");
                });

            modelBuilder.Entity("Dros.Data.Models.Tag", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTimeOffset>("CreatedOn");

                    b.Property<string>("Name");

                    b.Property<DateTimeOffset>("UpdatedOn");

                    b.HasKey("Id");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("Dros.Data.Models.Link", b =>
                {
                    b.HasOne("Dros.Data.Models.Material", "Material")
                        .WithMany("Links")
                        .HasForeignKey("MaterialId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Dros.Data.Models.MaterialAuthor", b =>
                {
                    b.HasOne("Dros.Data.Models.Author", "Author")
                        .WithMany("Materials")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Dros.Data.Models.Material", "Material")
                        .WithMany("Authors")
                        .HasForeignKey("MaterialId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Dros.Data.Models.MaterialCategory", b =>
                {
                    b.HasOne("Dros.Data.Models.Category", "Category")
                        .WithMany("Materials")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Dros.Data.Models.Material", "Material")
                        .WithMany("Categories")
                        .HasForeignKey("MaterialId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Dros.Data.Models.MaterialTag", b =>
                {
                    b.HasOne("Dros.Data.Models.Material", "Material")
                        .WithMany("Tags")
                        .HasForeignKey("MaterialId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Dros.Data.Models.Tag", "Tag")
                        .WithMany("Materials")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}