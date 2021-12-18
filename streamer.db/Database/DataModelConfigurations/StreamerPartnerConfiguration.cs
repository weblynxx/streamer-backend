using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using streamer.db.Database.DataModel;

namespace streamer.db.Database.DataModelConfigurations
{
    public class StreamerPartnerConfiguration : IEntityTypeConfiguration<StreamerPartnereDm>
    {
        public void Configure(EntityTypeBuilder<StreamerPartnereDm> builder)
        {
            builder.ToTable("StreamerPartner");
            builder.HasKey(x => x.Id);
            builder.HasOne(pt => pt.Streamer)
                .WithMany(p => p.StreamerPartners)
                .HasForeignKey(pt => pt.StreamerId);
        }
    }
}
