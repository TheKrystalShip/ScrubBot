using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ScrubBot.Database.Domain;

namespace ScrubBot.Database.SQLite.Configurations
{
    internal class GuildConfiguration : IEntityTypeConfiguration<Guild>
    {
        public void Configure(EntityTypeBuilder<Guild> builder)
        {
            builder.HasMany(x => x.Users)
                .WithOne(x => x.Guild)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Events)
                .WithOne(x => x.Guild);

            builder.Property(x => x.Id)
                .HasConversion<string>();

            builder.Property(x => x.AuditChannelId)
                .HasConversion<string>();
        }
    }
}
