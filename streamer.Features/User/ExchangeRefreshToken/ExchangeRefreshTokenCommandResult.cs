using System.Collections.Generic;
using System.Linq;
using streamer.Features.User;
using timeOrg.Features.User;

namespace timeOrg.Features.User.ExchangeRefreshToken
{
    public class ExchangeRefreshTokenCommandResult
    {
        public AccessToken Token { get; }
        public string RefreshToken { get; }
        private IEnumerable<Error> _errors;
        public IEnumerable<Error> Errors
        {
            get { return _errors ?? Enumerable.Empty<Error>(); }
        }

        public ExchangeRefreshTokenCommandResult(IEnumerable<Error> errors)
        {
            _errors = errors;
        }

        public ExchangeRefreshTokenCommandResult(AccessToken accessToken, string refreshToken) 
        {
            Token = accessToken;
            RefreshToken = refreshToken;
        }
    }
}