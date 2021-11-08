using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace streamer.db.Database.DataModel
{
    [Table("RefreshToken")]
    public class RefreshTokenDm
    {
        [Key]
        public Guid Id { get; set; }

        [MaxLength(32)]
        public string Token { get; private set; }
        public DateTime Expires { get; private set; }
        public Guid UserId { get; private set; }
        public bool Active => DateTime.UtcNow <= Expires;
        public string RemoteIpAddress { get; private set; }

        public RefreshTokenDm(string token, DateTime expires, Guid userId, string remoteIpAddress)
        {
            Token = token;
            Expires = expires;
            UserId = userId;
            RemoteIpAddress = remoteIpAddress;
        }
    }

}
