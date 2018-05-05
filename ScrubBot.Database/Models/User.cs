using System;
using System.ComponentModel.DataAnnotations;

namespace ScrubBot.Database.Models
{
    public class User
    {
        /// <summary>The User's ulong ID, mapped to a string.</summary>
        [Key, MaxLength(20)]
        public string Id { get; set; }
        [MaxLength(50)]
        public string Username { get; set; }
        [MaxLength(50)]
        public string Nickname { get; set; }
        [MaxLength(20)]
        public string Discriminator { get; set; }
        public string AvatarUrl { get; set; }
        public int? TimezoneOffset { get; set; } = null;

        public virtual Guild Guild { get; set; }
    }
}