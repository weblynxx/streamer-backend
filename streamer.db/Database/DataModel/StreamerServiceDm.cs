using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace streamer.db.Database.DataModel
{
    [Table("StreamerService")]
    public class StreamerServiceDm
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id")]
        [Description("Id")]
        public Guid Id { get; set; }
        public Guid StreamerId { get; set; }
        public StreamerDm Streamer { get; set; }

        public Guid ServiceId { get; set; }
        public ServiceDm Service { get; set; }
        public string ServiceUserName { get; set; }
    }


}
