using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ScrubBot.Database.Models
{
    public class Guild
    {
        /// <summary>The Guild's ulong ID, mapped to a string.</summary>
        [Key]
        public string Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        public string IconUrl { get; set; }
        public int MemberCount { get; set; }
        public string AuditChannelId { get; set; }
        public string CharPrefix { get; set; } = "#";
        public string StringPrefix { get; set; } = "ScrubBot, ";

        public virtual List<User> Users { get; set; } = new List<User>();
    }
}