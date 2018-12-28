using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ScrubBot.Database.Domain
{
    public class Guild
    {
        [Key]
        public ulong Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        public string IconUrl { get; set; }
        public int MemberCount { get; set; }
        public ulong? AuditChannelId { get; set; }
        public string Prefix { get; set; }

        public virtual List<User> Users { get; set; } = new List<User>();

        public virtual List<Event> Events { get; set; } = new List<Event>();
    }
}
