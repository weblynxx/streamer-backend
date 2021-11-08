using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace streamer.db.Database.DataModel
{
    [Table("Service")]
    public class ServiceDm
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id")]
        [Description("Id")]
        public Guid Id { get; set; }

        public string Name { get; set; }
        public virtual ICollection<StreamerServiceDm> StreamerServices { get; set; }
    }
}