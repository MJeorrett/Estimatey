﻿// <auto-generated />
using System;
using Estimatey.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Estimatey.Infrastructure.Persistence.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Estimatey.Core.Entities.FeatureEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("FeatureId");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("DevOpsId")
                        .HasColumnType("int");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("nvarchar(16)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("DevOpsId")
                        .IsUnique();

                    b.ToTable("Feature", (string)null);
                });

            modelBuilder.Entity("Estimatey.Core.Entities.FeatureTagEntity", b =>
                {
                    b.Property<int>("FeatureId")
                        .HasColumnType("int");

                    b.Property<int>("TagId")
                        .HasColumnType("int");

                    b.HasKey("FeatureId", "TagId");

                    b.HasIndex("TagId");

                    b.ToTable("FeatureTag", (string)null);
                });

            modelBuilder.Entity("Estimatey.Core.Entities.ProjectEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ProjectId");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("DevOpsContinuationToken")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DevOpsProjectName")
                        .IsRequired()
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.ToTable("Project", (string)null);
                });

            modelBuilder.Entity("Estimatey.Core.Entities.TagEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("TagId");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Tag", (string)null);
                });

            modelBuilder.Entity("Estimatey.Core.Entities.TicketEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("TicketId");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("DevOpsId")
                        .HasColumnType("int");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("nvarchar(16)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UserStoryId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DevOpsId")
                        .IsUnique();

                    b.HasIndex("UserStoryId");

                    b.ToTable("Ticket", (string)null);
                });

            modelBuilder.Entity("Estimatey.Core.Entities.TicketTagEntity", b =>
                {
                    b.Property<int>("TagId")
                        .HasColumnType("int");

                    b.Property<int>("TicketId")
                        .HasColumnType("int");

                    b.HasKey("TagId", "TicketId");

                    b.HasIndex("TicketId");

                    b.ToTable("TicketTag", (string)null);
                });

            modelBuilder.Entity("Estimatey.Core.Entities.UserStoryEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("UserStoryId");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("DevOpsId")
                        .HasColumnType("int");

                    b.Property<int?>("FeatureId")
                        .HasColumnType("int");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("nvarchar(16)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("DevOpsId")
                        .IsUnique();

                    b.HasIndex("FeatureId");

                    b.ToTable("UserStory", (string)null);
                });

            modelBuilder.Entity("Estimatey.Core.Entities.UserStoryTagEntity", b =>
                {
                    b.Property<int>("TagId")
                        .HasColumnType("int");

                    b.Property<int>("UserStoryId")
                        .HasColumnType("int");

                    b.HasKey("TagId", "UserStoryId");

                    b.HasIndex("UserStoryId");

                    b.ToTable("UserStoryTag", (string)null);
                });

            modelBuilder.Entity("Estimatey.Core.Entities.FeatureTagEntity", b =>
                {
                    b.HasOne("Estimatey.Core.Entities.FeatureEntity", null)
                        .WithMany()
                        .HasForeignKey("FeatureId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Estimatey.Core.Entities.TagEntity", null)
                        .WithMany()
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Estimatey.Core.Entities.TicketEntity", b =>
                {
                    b.HasOne("Estimatey.Core.Entities.UserStoryEntity", "UserStory")
                        .WithMany("Tickets")
                        .HasForeignKey("UserStoryId");

                    b.Navigation("UserStory");
                });

            modelBuilder.Entity("Estimatey.Core.Entities.TicketTagEntity", b =>
                {
                    b.HasOne("Estimatey.Core.Entities.TagEntity", null)
                        .WithMany()
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Estimatey.Core.Entities.TicketEntity", null)
                        .WithMany()
                        .HasForeignKey("TicketId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Estimatey.Core.Entities.UserStoryEntity", b =>
                {
                    b.HasOne("Estimatey.Core.Entities.FeatureEntity", "Feature")
                        .WithMany("UserStories")
                        .HasForeignKey("FeatureId");

                    b.Navigation("Feature");
                });

            modelBuilder.Entity("Estimatey.Core.Entities.UserStoryTagEntity", b =>
                {
                    b.HasOne("Estimatey.Core.Entities.TagEntity", null)
                        .WithMany()
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Estimatey.Core.Entities.UserStoryEntity", null)
                        .WithMany()
                        .HasForeignKey("UserStoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Estimatey.Core.Entities.FeatureEntity", b =>
                {
                    b.Navigation("UserStories");
                });

            modelBuilder.Entity("Estimatey.Core.Entities.UserStoryEntity", b =>
                {
                    b.Navigation("Tickets");
                });
#pragma warning restore 612, 618
        }
    }
}
