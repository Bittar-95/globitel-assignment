﻿// <auto-generated />
using ASSIGNMENT.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace ASSIGNMENT.Migrations
{
    [DbContext(typeof(EmployeeDbContext))]
    [Migration("20191217200120_init")]
    partial class init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.3-rtm-10026")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ASSIGNMENT.Models.Employee", b =>
                {
                    b.Property<int>("EmployeeID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("EmStatus");

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<int>("Gender");

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<DateTime>("MyProperty");

                    b.Property<int?>("User");

                    b.HasKey("EmployeeID");

                    b.HasIndex("User");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("ASSIGNMENT.Models.User", b =>
                {
                    b.Property<int>("UserID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConfirmPassword")
                        .IsRequired();

                    b.Property<string>("Password")
                        .IsRequired();

                    b.Property<string>("UserName")
                        .IsRequired();

                    b.HasKey("UserID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ASSIGNMENT.Models.Employee", b =>
                {
                    b.HasOne("ASSIGNMENT.Models.User", "AdminID")
                        .WithMany()
                        .HasForeignKey("User");
                });
#pragma warning restore 612, 618
        }
    }
}
