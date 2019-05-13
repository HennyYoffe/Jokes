﻿// <auto-generated />
using System;
using ClassLibrary1;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ClassLibrary1.Migrations
{
    [DbContext(typeof(JokesContext))]
    [Migration("20190513035746_id")]
    partial class id
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.0-rtm-30799")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ClassLibrary1.Joke", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("JokeDataId");

                    b.Property<string>("Punchline");

                    b.Property<string>("Setup");

                    b.HasKey("Id");

                    b.ToTable("Jokes");
                });

            modelBuilder.Entity("ClassLibrary1.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email");

                    b.Property<string>("Name");

                    b.Property<string>("PasswordHash");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ClassLibrary1.UserLikeJoke", b =>
                {
                    b.Property<int>("JokeId");

                    b.Property<int>("UserId");

                    b.Property<DateTime>("DateTime");

                    b.Property<bool>("Like");

                    b.HasKey("JokeId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("LikeJokes");
                });

            modelBuilder.Entity("ClassLibrary1.UserLikeJoke", b =>
                {
                    b.HasOne("ClassLibrary1.Joke", "Joke")
                        .WithMany("LikeJokes")
                        .HasForeignKey("JokeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ClassLibrary1.User", "User")
                        .WithMany("LikeJokes")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
