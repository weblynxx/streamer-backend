using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace streamer.db.Database.DataModel
{
    [Table("Streamer")]
    public class StreamerDm : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }

        
        // Mowscow
        public string City { get; set; }
        // Lenina
        public string Street { get; set; }
        // 5
        public string House { get; set; }
        // korpus - 2
        public string HouseBuilding { get; set; }
        // 10
        public string Entrance { get; set; }
        //5
        public int Floor { get; set; }
        // 115b
        public string Flat { get; set; }
        public string IntercomCode { get; set; }

        public string Authorities { get; set; }

        public TimeSpan From { get; set; }
        public TimeSpan To { get; set; }
        public bool isStoppedDelivery { get; set; }

        public string FoodPreferenceText { get; set; }
        public string ClothesPreferenceText { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime LastLoginDate { get; set; }
        public Guid StreamerId { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }

        public virtual ICollection<StreamerServiceDm> StreamerServices { get; set; }
    }

    public class PartnerDm //external
    {
        [Key]
        [ForeignKey("StreamerDm")]
        public Guid Id { get; set; }

        public string DeliveryName { get; set; }
        public DeliveryType Type { get; set; }

        public Guid? StreamerId { get; set; }
        [CsvHelper.Configuration.Attributes.Ignore]
        [JsonIgnore]
        [IgnoreDataMember]
        public virtual StreamerDm Streamer { get; set; }

    }

    public enum DeliveryType
    {
        Food = 0,
        Clothes = 1,
    }




}
