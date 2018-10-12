using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ScrubBot.Database.Models
{
    public class Event
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public virtual User Author { get; set; }

        public virtual Guild Guild { get; set; }

        public DateTime CreationDate { get; set; } = DateTime.UtcNow;

        public DateTime OccurenceDate { get; set; }

        public virtual List<User> Subscribers { get; set; } = new List<User>();

        public string Title { get; set; }

        public string Description { get; set; }

        public int MaxSubscribers { get; set; }
    }
}