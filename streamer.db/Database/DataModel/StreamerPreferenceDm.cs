using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace streamer.db.Database.DataModel
{
    [Table("StreamerPreference")]
    public class StreamerPreferenceDm
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id")]
        [Description("Id")]
        public Guid Id { get; set; }

        public Guid StreamerId { get; set; }
        public StreamerDm Streamer { get; set; }

        public Guid PreferenceId { get; set; }
        public PreferenceDm Preference { get; set; }
    }

   
}