using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace streamer.db.Database.DataModel
{
    [Table("Preference")]
    public class PreferenceDm
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id")]
        [Description("Id")]
        public Guid Id { get; set; }
        public PreferenceType Type { get; set; }

        public string Name { get; set; }
    }

    public enum PreferenceType
    {
        Food = 0,
        Clothes = 1
    }
}