using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Microsoft.EntityFrameworkCore;

using Discord;
using ScrubBot.Properties;

namespace ScrubBot.Data
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Guild> Guilds { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Resources.ConnectionString);
            optionsBuilder.EnableSensitiveDataLogging(true);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Guild>()
                .HasMany(x => x.Users);
        }
    }

    public class Guild
    {
        /// <summary>The Guild's ulong ID, mapped to a string.</summary>
        [Key, MaxLength(20)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string CharPrefix { get; set; } = "#";
        public string StringPrefix { get; set; } = "ScrubBot, ";
        public virtual List<User> Users { get; set; } = new List<User>();
    }

    public class User
    {
        /// <summary>The User's ulong ID, mapped to a string.</summary>
        [Key, MaxLength(20)]
        public string Id { get; set; }
        public string Username { get; set; }
        public string Nickname { get; set; }
    }
}