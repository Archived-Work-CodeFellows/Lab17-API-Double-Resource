﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ToDoApi.Data;

namespace ToDoApi.Migrations
{
    [DbContext(typeof(ToDoDbContext))]
    [Migration("20180628180011_Updated-item-field")]
    partial class Updateditemfield
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.0-rtm-30799")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ToDoApi.Models.ToDoItem", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("IsDone");

                    b.Property<long>("ListID");

                    b.Property<string>("Name");

                    b.Property<string>("ToDoList");

                    b.Property<int?>("ToDoListID");

                    b.HasKey("ID");

                    b.HasIndex("ToDoListID");

                    b.ToTable("ToDoItems");
                });

            modelBuilder.Entity("ToDoApi.Models.ToDoList", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("IsDone");

                    b.Property<string>("Name");

                    b.HasKey("ID");

                    b.ToTable("ToDoLists");
                });

            modelBuilder.Entity("ToDoApi.Models.ToDoItem", b =>
                {
                    b.HasOne("ToDoApi.Models.ToDoList")
                        .WithMany("ToDoItems")
                        .HasForeignKey("ToDoListID");
                });
#pragma warning restore 612, 618
        }
    }
}
