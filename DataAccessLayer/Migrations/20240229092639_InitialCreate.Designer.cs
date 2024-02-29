﻿// <auto-generated />
using System;
using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DataAccessLayer.Migrations
{
    [DbContext(typeof(ArtShareContext))]
    [Migration("20240229092639_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.26")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("ModelLayer.BussinessObject.Account", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Avatar")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<DateTime?>("Birthday")
                        .HasColumnType("datetime");

                    b.Property<DateTime?>("CreateDate")
                        .HasColumnType("datetime");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("FullName")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Password")
                        .HasMaxLength(16)
                        .HasColumnType("nvarchar(16)");

                    b.Property<string>("Status")
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("UserName")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Id");

                    b.ToTable("Account", (string)null);
                });

            modelBuilder.Entity("ModelLayer.BussinessObject.Artwork", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("AccountId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal?>("Fee")
                        .HasColumnType("decimal(18,0)");

                    b.Property<int?>("Likes")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("Title")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Url")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.ToTable("Artwork", (string)null);
                });

            modelBuilder.Entity("ModelLayer.BussinessObject.ArtworkCategory", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ArtworkId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CategoryId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ArtworkId");

                    b.HasIndex("CategoryId");

                    b.ToTable("ArtworkCategory", (string)null);
                });

            modelBuilder.Entity("ModelLayer.BussinessObject.ArtworkTag", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ArtworkId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("TagId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ArtworkId");

                    b.HasIndex("TagId");

                    b.ToTable("ArtworkTag", (string)null);
                });

            modelBuilder.Entity("ModelLayer.BussinessObject.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("CreateDate")
                        .HasColumnType("datetime");

                    b.Property<string>("Title")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Id");

                    b.ToTable("Category", (string)null);
                });

            modelBuilder.Entity("ModelLayer.BussinessObject.Comment", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("AccountId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ArtworkId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreateDate")
                        .HasColumnType("datetime");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("ArtworkId");

                    b.ToTable("Comment", (string)null);
                });

            modelBuilder.Entity("ModelLayer.BussinessObject.Follow", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ArtistId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("CreateDate")
                        .HasColumnType("datetime");

                    b.Property<Guid?>("FollowerId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ArtistId");

                    b.HasIndex("FollowerId");

                    b.ToTable("Follow", (string)null);
                });

            modelBuilder.Entity("ModelLayer.BussinessObject.Inbox", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreateDate")
                        .HasColumnType("datetime");

                    b.Property<Guid?>("ReceiverId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("SenderId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Title")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("ReceiverId");

                    b.HasIndex("SenderId");

                    b.ToTable("Inbox", (string)null);
                });

            modelBuilder.Entity("ModelLayer.BussinessObject.Like", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("AccountId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ArtworkId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("CreateDate")
                        .HasColumnType("datetime");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("ArtworkId");

                    b.ToTable("Likes");
                });

            modelBuilder.Entity("ModelLayer.BussinessObject.Order", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("AccountId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("CreateDate")
                        .HasColumnType("datetime");

                    b.Property<DateTime?>("PaymentDate")
                        .HasColumnType("datetime");

                    b.Property<string>("PaymentMethod")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Status")
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<decimal?>("TotalFee")
                        .HasColumnType("decimal(18,0)");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("ModelLayer.BussinessObject.OrderDetail", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ArtworkId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("OrderId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,0)");

                    b.HasKey("Id");

                    b.HasIndex("ArtworkId");

                    b.HasIndex("OrderId");

                    b.ToTable("OrderDetail", (string)null);
                });

            modelBuilder.Entity("ModelLayer.BussinessObject.Tag", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("CreateDate")
                        .HasColumnType("datetime");

                    b.Property<string>("Title")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Id");

                    b.ToTable("Tag", (string)null);
                });

            modelBuilder.Entity("ModelLayer.BussinessObject.Artwork", b =>
                {
                    b.HasOne("ModelLayer.BussinessObject.Account", "Account")
                        .WithMany("Artworks")
                        .HasForeignKey("AccountId")
                        .HasConstraintName("FK__Artwork__Account__4BAC3F29");

                    b.Navigation("Account");
                });

            modelBuilder.Entity("ModelLayer.BussinessObject.ArtworkCategory", b =>
                {
                    b.HasOne("ModelLayer.BussinessObject.Artwork", "Artwork")
                        .WithMany("ArtworkCategories")
                        .HasForeignKey("ArtworkId")
                        .HasConstraintName("FK__ArtworkCa__Artwo__60A75C0F");

                    b.HasOne("ModelLayer.BussinessObject.Category", "Category")
                        .WithMany("ArtworkCategories")
                        .HasForeignKey("CategoryId")
                        .HasConstraintName("FK__ArtworkCa__Categ__619B8048");

                    b.Navigation("Artwork");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("ModelLayer.BussinessObject.ArtworkTag", b =>
                {
                    b.HasOne("ModelLayer.BussinessObject.Artwork", "Artwork")
                        .WithMany("ArtworkTags")
                        .HasForeignKey("ArtworkId")
                        .HasConstraintName("FK__ArtworkTa__Artwo__6477ECF3");

                    b.HasOne("ModelLayer.BussinessObject.Tag", "Tag")
                        .WithMany("ArtworkTags")
                        .HasForeignKey("TagId")
                        .HasConstraintName("FK__ArtworkTa__TagId__656C112C");

                    b.Navigation("Artwork");

                    b.Navigation("Tag");
                });

            modelBuilder.Entity("ModelLayer.BussinessObject.Comment", b =>
                {
                    b.HasOne("ModelLayer.BussinessObject.Account", "Account")
                        .WithMany("Comments")
                        .HasForeignKey("AccountId")
                        .HasConstraintName("FK__Comment__Account__59063A47");

                    b.HasOne("ModelLayer.BussinessObject.Artwork", "Artwork")
                        .WithMany("Comments")
                        .HasForeignKey("ArtworkId")
                        .HasConstraintName("FK__Comment__Artwork__59FA5E80");

                    b.Navigation("Account");

                    b.Navigation("Artwork");
                });

            modelBuilder.Entity("ModelLayer.BussinessObject.Follow", b =>
                {
                    b.HasOne("ModelLayer.BussinessObject.Account", "Artist")
                        .WithMany("FollowArtists")
                        .HasForeignKey("ArtistId")
                        .HasConstraintName("FK__Follow__ArtistId__52593CB8");

                    b.HasOne("ModelLayer.BussinessObject.Account", "Follower")
                        .WithMany("FollowFollowers")
                        .HasForeignKey("FollowerId")
                        .HasConstraintName("FK__Follow__Follower__5165187F");

                    b.Navigation("Artist");

                    b.Navigation("Follower");
                });

            modelBuilder.Entity("ModelLayer.BussinessObject.Inbox", b =>
                {
                    b.HasOne("ModelLayer.BussinessObject.Account", "Receiver")
                        .WithMany("InboxReceivers")
                        .HasForeignKey("ReceiverId")
                        .HasConstraintName("FK__Inbox__ReceiverI__70DDC3D8");

                    b.HasOne("ModelLayer.BussinessObject.Account", "Sender")
                        .WithMany("InboxSenders")
                        .HasForeignKey("SenderId")
                        .HasConstraintName("FK__Inbox__SenderId__6FE99F9F");

                    b.Navigation("Receiver");

                    b.Navigation("Sender");
                });

            modelBuilder.Entity("ModelLayer.BussinessObject.Like", b =>
                {
                    b.HasOne("ModelLayer.BussinessObject.Account", "Account")
                        .WithMany("Likes")
                        .HasForeignKey("AccountId")
                        .HasConstraintName("FK__Likes__AccountId__5535A963");

                    b.HasOne("ModelLayer.BussinessObject.Artwork", "Artwork")
                        .WithMany("LikesNavigation")
                        .HasForeignKey("ArtworkId")
                        .HasConstraintName("FK__Likes__ArtworkId__5629CD9C");

                    b.Navigation("Account");

                    b.Navigation("Artwork");
                });

            modelBuilder.Entity("ModelLayer.BussinessObject.Order", b =>
                {
                    b.HasOne("ModelLayer.BussinessObject.Account", "Account")
                        .WithMany("Orders")
                        .HasForeignKey("AccountId")
                        .HasConstraintName("FK__Orders__AccountI__4E88ABD4");

                    b.Navigation("Account");
                });

            modelBuilder.Entity("ModelLayer.BussinessObject.OrderDetail", b =>
                {
                    b.HasOne("ModelLayer.BussinessObject.Artwork", "Artwork")
                        .WithMany("OrderDetails")
                        .HasForeignKey("ArtworkId")
                        .HasConstraintName("FK__OrderDeta__Artwo__693CA210");

                    b.HasOne("ModelLayer.BussinessObject.Order", "Order")
                        .WithMany("OrderDetails")
                        .HasForeignKey("OrderId")
                        .HasConstraintName("FK__OrderDeta__Order__68487DD7");

                    b.Navigation("Artwork");

                    b.Navigation("Order");
                });

            modelBuilder.Entity("ModelLayer.BussinessObject.Account", b =>
                {
                    b.Navigation("Artworks");

                    b.Navigation("Comments");

                    b.Navigation("FollowArtists");

                    b.Navigation("FollowFollowers");

                    b.Navigation("InboxReceivers");

                    b.Navigation("InboxSenders");

                    b.Navigation("Likes");

                    b.Navigation("Orders");
                });

            modelBuilder.Entity("ModelLayer.BussinessObject.Artwork", b =>
                {
                    b.Navigation("ArtworkCategories");

                    b.Navigation("ArtworkTags");

                    b.Navigation("Comments");

                    b.Navigation("LikesNavigation");

                    b.Navigation("OrderDetails");
                });

            modelBuilder.Entity("ModelLayer.BussinessObject.Category", b =>
                {
                    b.Navigation("ArtworkCategories");
                });

            modelBuilder.Entity("ModelLayer.BussinessObject.Order", b =>
                {
                    b.Navigation("OrderDetails");
                });

            modelBuilder.Entity("ModelLayer.BussinessObject.Tag", b =>
                {
                    b.Navigation("ArtworkTags");
                });
#pragma warning restore 612, 618
        }
    }
}
