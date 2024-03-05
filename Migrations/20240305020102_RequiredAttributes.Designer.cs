﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TunaPiano;

#nullable disable

namespace TunaPiano.Migrations
{
    [DbContext(typeof(TunaPianoDbContext))]
    [Migration("20240305020102_RequiredAttributes")]
    partial class RequiredAttributes
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("GenreSong", b =>
                {
                    b.Property<int>("GenresId")
                        .HasColumnType("integer");

                    b.Property<int>("SongsId")
                        .HasColumnType("integer");

                    b.HasKey("GenresId", "SongsId");

                    b.HasIndex("SongsId");

                    b.ToTable("GenreSong");
                });

            modelBuilder.Entity("TunaPiano.Models.Artist", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("Age")
                        .HasColumnType("integer");

                    b.Property<string>("Bio")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Artists");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Age = 33,
                            Bio = "belt",
                            Name = "Travis Scott"
                        },
                        new
                        {
                            Id = 2,
                            Age = 27,
                            Bio = "genre defying artist wow so good",
                            Name = "Jean Dawson"
                        },
                        new
                        {
                            Id = 3,
                            Age = 31,
                            Bio = "smooth chill sometimes hype sometimes cry",
                            Name = "No Rome"
                        },
                        new
                        {
                            Id = 4,
                            Age = 28,
                            Bio = "dominic fike brooo",
                            Name = "Dominic Fike"
                        });
                });

            modelBuilder.Entity("TunaPiano.Models.Genre", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Genres");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Description = "Hip-Hop/Rap"
                        },
                        new
                        {
                            Id = 2,
                            Description = "Alternative"
                        },
                        new
                        {
                            Id = 3,
                            Description = "Indie Pop"
                        });
                });

            modelBuilder.Entity("TunaPiano.Models.Song", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Album")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("ArtistId")
                        .HasColumnType("integer");

                    b.Property<int>("Length")
                        .HasColumnType("integer");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Songs");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Album = "Pixel Bath",
                            ArtistId = 2,
                            Length = 3,
                            Title = "Devilish"
                        },
                        new
                        {
                            Id = 2,
                            Album = "Utopia",
                            ArtistId = 1,
                            Length = 3,
                            Title = "Sirens"
                        },
                        new
                        {
                            Id = 3,
                            Album = "It's All Smiles",
                            ArtistId = 3,
                            Length = 2,
                            Title = "ITS *NOT* L0V33 (Winter in London)"
                        },
                        new
                        {
                            Id = 4,
                            Album = "Birds in the Trap Sing McKnight",
                            ArtistId = 1,
                            Length = 4,
                            Title = "the ends"
                        },
                        new
                        {
                            Id = 5,
                            Album = "It's All Smiles",
                            ArtistId = 3,
                            Length = 3,
                            Title = "When She Comes Around"
                        },
                        new
                        {
                            Id = 6,
                            Album = "Sunburn",
                            ArtistId = 4,
                            Length = 2,
                            Title = "Dancing In The Courthouse"
                        });
                });

            modelBuilder.Entity("GenreSong", b =>
                {
                    b.HasOne("TunaPiano.Models.Genre", null)
                        .WithMany()
                        .HasForeignKey("GenresId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TunaPiano.Models.Song", null)
                        .WithMany()
                        .HasForeignKey("SongsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
