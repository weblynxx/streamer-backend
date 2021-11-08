using System.Collections.Generic;
using System.Linq;

namespace streamer.Features.User.Login
{
    public class LoginCommandResult
    {
        public AccessToken Token { get; }
        public string RefreshToken { get; }
        private IEnumerable<Error> _errors;
        public IEnumerable<Error> Errors { get{return _errors?? Enumerable.Empty<Error>();}
}

        public LoginCommandResult(IEnumerable<Error> errors) 
        {
            _errors = errors;
        }

        public LoginCommandResult(AccessToken token, string refreshToken)  
        {
            Token = token;
            RefreshToken = refreshToken;
        }
    }
}