using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace streamer.db.Database.DataModel
{
    [Table("StreamerPartner")]
    public class StreamerPartnereDm
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id")]
        [Description("Id")]
        public Guid Id { get; set; }

        public Guid StreamerId { get; set; }
        public StreamerDm Streamer { get; set; }

        public Guid PartnerId { get; set; }
        public PartnerDm Partner { get; set; }
    }

   
}