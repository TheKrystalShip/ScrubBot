using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ScrubBot.Database.Domain;

namespace ScrubBot.Database.SQLite.Configurations
{
    internal class EventConfiguration : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.HasMany(x => x.Subscribers);

            builder.HasOne(x => x.Author)
                .WithMany(x => x.AuthoringEvents);

            builder.Property(x => x.SubscribeMessageId)
                .HasConversion<string>();
        }
    }
}
