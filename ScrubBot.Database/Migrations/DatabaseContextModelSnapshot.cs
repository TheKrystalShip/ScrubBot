﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using ScrubBot.Database;
using System;

namespace ScrubBot.Database.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    partial class DatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.2-rtm-10011")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ScrubBot.Database.Models.Guild", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AuditChannelId");

                    b.Property<string>("CharPrefix");

                    b.Property<string>("IconUrl");

                    b.Property<int>("MemberCount");

                    b.Property<string>("Name")
                        .HasMaxLength(100);

                    b.Property<string>("StringPrefix");

                    b.HasKey("Id");

                    b.ToTable("Guilds");
                });

            modelBuilder.Entity("ScrubBot.Database.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(20);

                    b.Property<string>("AvatarUrl");

                    b.Property<string>("Discriminator")
                        .HasMaxLength(20);

                    b.Property<string>("GuildId");

                    b.Property<string>("Nickname")
                        .HasMaxLength(50);

                    b.Property<int?>("TimezoneOffset");

                    b.Property<string>("Username")
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.HasIndex("GuildId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ScrubBot.Database.Models.User", b =>
                {
                    b.HasOne("ScrubBot.Database.Models.Guild", "Guild")
                        .WithMany("Users")
                        .HasForeignKey("GuildId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
