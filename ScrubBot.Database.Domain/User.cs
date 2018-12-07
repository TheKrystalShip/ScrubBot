using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ScrubBot.Database.Domain
{
    public class User
    {
        [Key]
        public ulong Id { get; set; }

        [MaxLength(50)]
        public string Username { get; set; }

        [MaxLength(50)]
        public string Nickname { get; set; }

        [MaxLength(20)]
        public string Discriminator { get; set; }

        public string AvatarUrl { get; set; }

        public DateTime Birthday { get; set; }

        public virtual Guild Guild { get; set; }

        public virtual List<Event> AuthoringEvents { get; set; } = new List<Event>();

        public virtual List<Event> SubscribedEvents { get; set; } = new List<Event>();
        
        public bool IsBirthdayToday()
        {
            return Birthday.Date == DateTime.Today.Date;
        }
    }
}
