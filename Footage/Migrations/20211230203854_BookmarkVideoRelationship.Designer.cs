﻿// <auto-generated />
using System;
using Footage.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Footage.Migrations
{
    [DbContext(typeof(VideoContext))]
    [Migration("20211230203854_BookmarkVideoRelationship")]
    partial class BookmarkVideoRelationship
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.13");

            modelBuilder.Entity("Footage.Model.Bookmark", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("VideoId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("VideoId");

                    b.ToTable("Bookmarks");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Bookmark");
                });

            modelBuilder.Entity("Footage.Model.MediaSource", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Active")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Available")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("MediaSources");

                    b.HasDiscriminator<string>("Discriminator").HasValue("MediaSource");
                });

            modelBuilder.Entity("Footage.Model.Video", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<long>("Duration")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MediaSourceId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("MediaSourceUri")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("MediaSourceId");

                    b.ToTable("Videos");
                });

            modelBuilder.Entity("Footage.Model.VideoThumbnail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<byte[]>("Image")
                        .IsRequired()
                        .HasColumnType("BLOB");

                    b.Property<int>("VideoId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("VideoId")
                        .IsUnique();

                    b.ToTable("VideoThumbnail");
                });

            modelBuilder.Entity("Footage.Model.RangeBookmark", b =>
                {
                    b.HasBaseType("Footage.Model.Bookmark");

                    b.Property<long>("EndTime")
                        .HasColumnType("INTEGER");

                    b.Property<long>("StartTime")
                        .HasColumnType("INTEGER");

                    b.HasDiscriminator().HasValue("RangeBookmark");
                });

            modelBuilder.Entity("Footage.Model.TimeBookmark", b =>
                {
                    b.HasBaseType("Footage.Model.Bookmark");

                    b.Property<long>("Time")
                        .HasColumnType("INTEGER");

                    b.HasDiscriminator().HasValue("TimeBookmark");
                });

            modelBuilder.Entity("Footage.Model.LocalMediaSource", b =>
                {
                    b.HasBaseType("Footage.Model.MediaSource");

                    b.Property<bool>("IncludeSubfolders")
                        .HasColumnType("INTEGER");

                    b.Property<string>("RootPath")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasDiscriminator().HasValue("LocalMediaSource");
                });

            modelBuilder.Entity("Footage.Model.Bookmark", b =>
                {
                    b.HasOne("Footage.Model.Video", "Video")
                        .WithMany("Bookmarks")
                        .HasForeignKey("VideoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Video");
                });

            modelBuilder.Entity("Footage.Model.Video", b =>
                {
                    b.HasOne("Footage.Model.MediaSource", "MediaSource")
                        .WithMany()
                        .HasForeignKey("MediaSourceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MediaSource");
                });

            modelBuilder.Entity("Footage.Model.VideoThumbnail", b =>
                {
                    b.HasOne("Footage.Model.Video", null)
                        .WithOne("Thumbnail")
                        .HasForeignKey("Footage.Model.VideoThumbnail", "VideoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Footage.Model.Video", b =>
                {
                    b.Navigation("Bookmarks");

                    b.Navigation("Thumbnail");
                });
#pragma warning restore 612, 618
        }
    }
}