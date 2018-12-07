using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ScrubBot.Database.Domain;

namespace ScrubBot.Database.SQLite.Configurations
{
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(x => x.Id)
                .HasConversion<string>();

            builder.HasMany(x => x.AuthoringEvents)
                .WithOne(x => x.Author);

            builder.HasMany(x => x.SubscribedEvents);
        }
    }
}
