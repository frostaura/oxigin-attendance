﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Oxigin.Attendance.Datastore;

#nullable disable

namespace Oxigin.Attendance.Datastore.Migrations
{
    [DbContext(typeof(DatastoreContext))]
    partial class DatastoreContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Oxigin.Attendance.Shared.Models.Entities.AdditionalWorker", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("Deleted")
                        .HasColumnType("boolean");

                    b.Property<Guid>("JobID")
                        .HasColumnType("uuid");

                    b.Property<int>("NumHours")
                        .HasColumnType("integer");

                    b.Property<int>("NumWorkers")
                        .HasColumnType("integer");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("WorkerType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("JobID");

                    b.ToTable("AdditionalWorkers");
                });

            modelBuilder.Entity("Oxigin.Attendance.Shared.Models.Entities.Allocation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("Deleted")
                        .HasColumnType("boolean");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<Guid>("EmployeeID")
                        .HasColumnType("uuid");

                    b.Property<int>("HoursNeeded")
                        .HasColumnType("integer");

                    b.Property<Guid>("JobID")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("Time")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeID");

                    b.HasIndex("JobID");

                    b.ToTable("Allocations");
                });

            modelBuilder.Entity("Oxigin.Attendance.Shared.Models.Entities.Client", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("CompanyName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ContactNo")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("Deleted")
                        .HasColumnType("boolean");

                    b.Property<string>("RegNo")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("Oxigin.Attendance.Shared.Models.Entities.Employee", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("AccountHolderName")
                        .HasColumnType("text");

                    b.Property<string>("AccountNumber")
                        .HasColumnType("text");

                    b.Property<string>("Address")
                        .HasColumnType("text");

                    b.Property<string>("BankName")
                        .HasColumnType("text");

                    b.Property<string>("BranchCode")
                        .HasColumnType("text");

                    b.Property<string>("ContactNo")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("Deleted")
                        .HasColumnType("boolean");

                    b.Property<string>("IDNumber")
                        .HasColumnType("text");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("Oxigin.Attendance.Shared.Models.Entities.Job", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("Approved")
                        .HasColumnType("boolean");

                    b.Property<Guid>("ClientID")
                        .HasColumnType("uuid");

                    b.Property<bool>("Deleted")
                        .HasColumnType("boolean");

                    b.Property<string>("JobName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("NumHours")
                        .HasColumnType("integer");

                    b.Property<int>("NumWorkers")
                        .HasColumnType("integer");

                    b.Property<string>("PurchaseOrderNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("RequestorID")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Time")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("ClientID");

                    b.HasIndex("RequestorID");

                    b.ToTable("Jobs");
                });

            modelBuilder.Entity("Oxigin.Attendance.Shared.Models.Entities.Oxigin", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("Deleted")
                        .HasColumnType("boolean");

                    b.Property<string>("Department")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Oxigin");
                });

            modelBuilder.Entity("Oxigin.Attendance.Shared.Models.Entities.Timesheet", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("Deleted")
                        .HasColumnType("boolean");

                    b.Property<Guid>("EmployeeID")
                        .HasColumnType("uuid");

                    b.Property<Guid>("JobID")
                        .HasColumnType("uuid");

                    b.Property<Guid>("SiteManagerID")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("TimeIn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("TimeOut")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeID");

                    b.HasIndex("JobID");

                    b.HasIndex("SiteManagerID");

                    b.ToTable("Timesheets");
                });

            modelBuilder.Entity("Oxigin.Attendance.Shared.Models.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("ClientID")
                        .HasColumnType("uuid");

                    b.Property<string>("ContactNr")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("Deleted")
                        .HasColumnType("boolean");

                    b.Property<string>("Department")
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid?>("EmployeeID")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("UserType")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ClientID");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("EmployeeID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Oxigin.Attendance.Shared.Models.Entities.UserSession", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("Deleted")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserSessions");
                });

            modelBuilder.Entity("Oxigin.Attendance.Shared.Models.Entities.AdditionalWorker", b =>
                {
                    b.HasOne("Oxigin.Attendance.Shared.Models.Entities.Job", "Job")
                        .WithMany()
                        .HasForeignKey("JobID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Job");
                });

            modelBuilder.Entity("Oxigin.Attendance.Shared.Models.Entities.Allocation", b =>
                {
                    b.HasOne("Oxigin.Attendance.Shared.Models.Entities.Employee", "Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Oxigin.Attendance.Shared.Models.Entities.Job", "Job")
                        .WithMany()
                        .HasForeignKey("JobID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");

                    b.Navigation("Job");
                });

            modelBuilder.Entity("Oxigin.Attendance.Shared.Models.Entities.Job", b =>
                {
                    b.HasOne("Oxigin.Attendance.Shared.Models.Entities.Client", "Client")
                        .WithMany()
                        .HasForeignKey("ClientID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Oxigin.Attendance.Shared.Models.Entities.User", "Requestor")
                        .WithMany()
                        .HasForeignKey("RequestorID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Client");

                    b.Navigation("Requestor");
                });

            modelBuilder.Entity("Oxigin.Attendance.Shared.Models.Entities.Timesheet", b =>
                {
                    b.HasOne("Oxigin.Attendance.Shared.Models.Entities.Employee", "Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Oxigin.Attendance.Shared.Models.Entities.Job", "Job")
                        .WithMany()
                        .HasForeignKey("JobID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Oxigin.Attendance.Shared.Models.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("SiteManagerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");

                    b.Navigation("Job");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Oxigin.Attendance.Shared.Models.Entities.User", b =>
                {
                    b.HasOne("Oxigin.Attendance.Shared.Models.Entities.Client", "Client")
                        .WithMany()
                        .HasForeignKey("ClientID");

                    b.HasOne("Oxigin.Attendance.Shared.Models.Entities.Employee", "Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeID");

                    b.Navigation("Client");

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("Oxigin.Attendance.Shared.Models.Entities.UserSession", b =>
                {
                    b.HasOne("Oxigin.Attendance.Shared.Models.Entities.User", "User")
                        .WithMany("Sessions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Oxigin.Attendance.Shared.Models.Entities.User", b =>
                {
                    b.Navigation("Sessions");
                });
#pragma warning restore 612, 618
        }
    }
}
