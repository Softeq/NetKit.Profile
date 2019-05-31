﻿// <auto-generated />

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Softeq.NetKit.Profile.SQLRepository.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20180410101630_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.2-rtm-10011")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Softeq.NetKit.Profile.Domain.Models.Profile.UserProfile", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Bio");

                    b.Property<DateTime>("Created");

                    b.Property<DateTime?>("DateOfBirth");

                    b.Property<string>("Email");

                    b.Property<string>("FirstName");

                    b.Property<int>("Gender");

                    b.Property<string>("LastName");

                    b.Property<double?>("Latitude");

                    b.Property<string>("Location");

                    b.Property<double?>("Longitude");

                    b.Property<string>("PhotoName");

                    b.Property<DateTime?>("Updated");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.ToTable("UserProfile");
                });
#pragma warning restore 612, 618
        }
    }
}