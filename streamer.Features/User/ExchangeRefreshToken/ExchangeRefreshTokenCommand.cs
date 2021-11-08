using MediatR;
using timeOrg.Features.User.ExchangeRefreshToken;

namespace streamer.Features.User.ExchangeRefreshToken
{
    public class ExchangeRefreshTokenCommand : IRequest<ExchangeRefreshTokenCommandResult>
    {
        public string AccessToken { get; }
        public string RefreshToken { get; }
        public string SigningKey { get; }

        public ExchangeRefreshTokenCommand(string accessToken, string refreshToken, string signingKey)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            SigningKey = signingKey;
        }
    }
}
