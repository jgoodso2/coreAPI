﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using ProjectInfo.API.Entities;
using System;

namespace ProjectInfo.API.Migrations
{
    [DbContext(typeof(ProjectContext))]
    partial class ProjectContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ProjectInfo.API.Entities.AuthorizedPlanViewProject", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<string>("Name");

                b.Property<string>("ppl_Code");

                b.HasKey("Id");

                b.ToTable("AuthorizedPlanViewProjects");
            });

            modelBuilder.Entity("ProjectInfo.API.Entities.PlanViewProject", b =>
            {
                b.Property<string>("ppl_Code")
                    .HasMaxLength(200);

                b.Property<Guid>("ProjectGuid");

                b.Property<string>("ProjectName")
                    .IsRequired()
                    .HasMaxLength(50);

                b.HasKey("ppl_Code");

                b.HasIndex("ProjectGuid");

                b.ToTable("PlanViewProjects");
            });

            modelBuilder.Entity("ProjectInfo.API.Entities.Project", b =>
            {
                b.Property<Guid>("ProjectGuid");

                b.Property<string>("ProjectName")
                    .IsRequired()
                    .HasMaxLength(50);

                b.HasKey("ProjectGuid");

                b.ToTable("Projects");
            });

            modelBuilder.Entity("ProjectInfo.API.Entities.PlanViewProject", b =>
            {
                b.HasOne("ProjectInfo.API.Entities.Project", "Project")
                    .WithMany("PlanViewProjects")
                    .HasForeignKey("ProjectGuid")
                    .OnDelete(DeleteBehavior.Cascade);
            });
#pragma warning restore 612, 618
        }
    }
}