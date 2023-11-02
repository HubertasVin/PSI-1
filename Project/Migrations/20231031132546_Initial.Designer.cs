﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Project.Data;

#nullable disable

namespace Project.Migrations
{
    [DbContext(typeof(NoteBlendDbContext))]
    [Migration("20231031132546_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Project.Models.Subject", b =>
                {
                    b.Property<string>("id")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("id");

                    b.ToTable("Subjects");
                });

            modelBuilder.Entity("Project.Models.Topic", b =>
                {
                    b.Property<string>("id")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Subjectid")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("id");

                    b.HasIndex("Subjectid");

                    b.ToTable("Topics");
                });

            modelBuilder.Entity("Project.Models.Topic", b =>
                {
                    b.HasOne("Project.Models.Subject", "Subject")
                        .WithMany()
                        .HasForeignKey("Subjectid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Subject");
                });
#pragma warning restore 612, 618
        }
    }
}
