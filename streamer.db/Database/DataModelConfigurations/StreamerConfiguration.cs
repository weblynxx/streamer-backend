﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using streamer.db.Database.DataModel;

namespace streamer.db.Database.DataModelConfigurations
{
    public class StreamerConfiguration : IEntityTypeConfiguration<StreamerDm>
    {
        public void Configure(EntityTypeBuilder<StreamerDm> builder)
        {
            builder.ToTable("Streamers");
            builder.HasKey(x => x.Id);
            builder.Property(p => p.Id).HasColumnName("Id");
            builder.Property(p => p.CreatedDate).Metadata.AfterSaveBehavior = PropertySaveBehavior.Ignore;
        }
    }
}
