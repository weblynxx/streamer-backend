using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using streamer.db.Database.DataModel;

namespace streamer.db.Database.DataModelConfigurations
{
    public class PartnerConfiguration : IEntityTypeConfiguration<PartnerDm>
    {
        public void Configure(EntityTypeBuilder<PartnerDm> builder)
        {
            builder.ToTable("Partner");
            builder.HasKey(x => x.Id);
            builder.Property(p => p.Id).HasColumnName("Id");
        }
    }
}
