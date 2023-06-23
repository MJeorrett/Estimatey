﻿// <auto-generated />
using System;
using Estimatey.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Estimatey.Infrastructure.Persistence.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230623143223_AddProjectIdToLoggedTime")]
    partial class AddProjectIdToLoggedTime
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Estimatey.Core.Entities.BugEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("BugId");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("ChangedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("DevOpsId")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Iteration")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("ProjectId")
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

                    b.HasIndex("ProjectId");

                    b.HasIndex("UserStoryId");

                    b.ToTable("Bug", (string)null);
                });

            modelBuilder.Entity("Estimatey.Core.Entities.BugTagEntity", b =>
                {
                    b.Property<int>("BugId")
                        .HasColumnType("int");

                    b.Property<int>("TagId")
                        .HasColumnType("int");

                    b.HasKey("BugId", "TagId");

                    b.HasIndex("TagId");

                    b.ToTable("BugTag", (string)null);
                });

            modelBuilder.Entity("Estimatey.Core.Entities.FeatureEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("FeatureId");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("ChangedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("DevOpsId")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Iteration")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("ProjectId")
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

                    b.HasIndex("ProjectId");

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

            modelBuilder.Entity("Estimatey.Core.Entities.FloatPersonEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("FloatPersonId");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("FloatId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.HasKey("Id");

                    b.HasIndex("FloatId")
                        .IsUnique();

                    b.ToTable("FloatPerson", (string)null);
                });

            modelBuilder.Entity("Estimatey.Core.Entities.LoggedTimeEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("LoggedTimeId");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("date");

                    b.Property<string>("FloatId")
                        .IsRequired()
                        .HasMaxLength(24)
                        .HasColumnType("nvarchar(24)");

                    b.Property<int>("FloatPersonId")
                        .HasColumnType("int");

                    b.Property<double>("Hours")
                        .HasColumnType("float");

                    b.Property<bool>("Locked")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LockedDate")
                        .HasColumnType("date");

                    b.Property<int>("ProjectId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FloatId")
                        .IsUnique();

                    b.HasIndex("FloatPersonId");

                    b.HasIndex("ProjectId");

                    b.ToTable("LoggedTime", (string)null);
                });

            modelBuilder.Entity("Estimatey.Core.Entities.ProjectEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ProjectId");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("DevOpsProjectName")
                        .IsRequired()
                        .HasColumnType("nvarchar(256)");

                    b.Property<int>("FloatId")
                        .HasColumnType("int");

                    b.Property<string>("LinksContinuationToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LoggedTimeHasBeenSyncedUntil")
                        .HasColumnType("date");

                    b.Property<string>("WorkItemsContinuationToken")
                        .HasColumnType("nvarchar(max)");

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

                    b.Property<DateTime>("ChangedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("DevOpsId")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Iteration")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("ProjectId")
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

                    b.HasIndex("ProjectId");

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

                    b.Property<DateTime>("ChangedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("DevOpsId")
                        .HasColumnType("int");

                    b.Property<int?>("FeatureId")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Iteration")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("ProjectId")
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

                    b.HasIndex("ProjectId");

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

            modelBuilder.Entity("Estimatey.Core.Entities.BugEntity", b =>
                {
                    b.HasOne("Estimatey.Core.Entities.ProjectEntity", "Project")
                        .WithMany()
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Estimatey.Core.Entities.UserStoryEntity", "UserStory")
                        .WithMany("Bugs")
                        .HasForeignKey("UserStoryId");

                    b.Navigation("Project");

                    b.Navigation("UserStory");
                });

            modelBuilder.Entity("Estimatey.Core.Entities.BugTagEntity", b =>
                {
                    b.HasOne("Estimatey.Core.Entities.BugEntity", null)
                        .WithMany()
                        .HasForeignKey("BugId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Estimatey.Core.Entities.TagEntity", null)
                        .WithMany()
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Estimatey.Core.Entities.FeatureEntity", b =>
                {
                    b.HasOne("Estimatey.Core.Entities.ProjectEntity", "Project")
                        .WithMany("Features")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Project");
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

            modelBuilder.Entity("Estimatey.Core.Entities.LoggedTimeEntity", b =>
                {
                    b.HasOne("Estimatey.Core.Entities.FloatPersonEntity", "FloatPerson")
                        .WithMany("LoggedTime")
                        .HasForeignKey("FloatPersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Estimatey.Core.Entities.ProjectEntity", "Project")
                        .WithMany("LoggedTime")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FloatPerson");

                    b.Navigation("Project");
                });

            modelBuilder.Entity("Estimatey.Core.Entities.TicketEntity", b =>
                {
                    b.HasOne("Estimatey.Core.Entities.ProjectEntity", "Project")
                        .WithMany("Tickets")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Estimatey.Core.Entities.UserStoryEntity", "UserStory")
                        .WithMany("Tickets")
                        .HasForeignKey("UserStoryId");

                    b.Navigation("Project");

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

                    b.HasOne("Estimatey.Core.Entities.ProjectEntity", "Project")
                        .WithMany("UserStories")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Feature");

                    b.Navigation("Project");
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

            modelBuilder.Entity("Estimatey.Core.Entities.FloatPersonEntity", b =>
                {
                    b.Navigation("LoggedTime");
                });

            modelBuilder.Entity("Estimatey.Core.Entities.ProjectEntity", b =>
                {
                    b.Navigation("Features");

                    b.Navigation("LoggedTime");

                    b.Navigation("Tickets");

                    b.Navigation("UserStories");
                });

            modelBuilder.Entity("Estimatey.Core.Entities.UserStoryEntity", b =>
                {
                    b.Navigation("Bugs");

                    b.Navigation("Tickets");
                });
#pragma warning restore 612, 618
        }
    }
}