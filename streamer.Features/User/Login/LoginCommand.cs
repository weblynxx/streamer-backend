using System.ComponentModel.DataAnnotations;
using MediatR;

namespace streamer.Features.User.Login
{
    public class LoginCommand : IRequest<LoginCommandResult>
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
        
        public string RemoteIpAddress { get; set; }

        
    }
}
