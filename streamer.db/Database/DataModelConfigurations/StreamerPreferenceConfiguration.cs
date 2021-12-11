using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using streamer.db.Database.DataModel;

namespace streamer.db.Database.DataModelConfigurations
{
    public class StreamerPreferenceConfiguration : IEntityTypeConfiguration<StreamerPreferenceDm>
    {
        public void Configure(EntityTypeBuilder<StreamerPreferenceDm> builder)
        {
            builder.ToTable("StreamerPreference");
            builder.HasKey(x => x.Id);
            builder.HasOne(pt => pt.Streamer)
                .WithMany(p => p.StreamerPreferences)
                .HasForeignKey(pt => pt.StreamerId);
        }
    }
}
