using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using streamer.db.Database.DataModel;

namespace streamer.db.Database.DataModelConfigurations
{
    public class StreamerServiceConfiguration: IEntityTypeConfiguration<StreamerServiceDm>
    {
        public void Configure(EntityTypeBuilder<StreamerServiceDm> builder)
        {
            builder.ToTable("StreamerService");
            builder.HasKey(x => x.Id);

            builder.HasOne(pt => pt.Streamer)
                .WithMany(p => p.StreamerServices)
                .HasForeignKey(pt => pt.StreamerId);

            builder.HasOne(pt => pt.Service)
                .WithMany(p => p.StreamerServices)
                .HasForeignKey(pt => pt.ServiceId);
        }
    }
}
