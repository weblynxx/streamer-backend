using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace streamer.db.Database.DataModel
{
    [Table("Streamer")]
    public class StreamerDm : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }

        
        public string City { get; set; }
        public string Street { get; set; }
        public string Authorities { get; set; }


        public DateTime CreatedDate { get; set; }
        public DateTime LastLoginDate { get; set; }
        public Guid StreamerId { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
    }


}
