using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using streamer.db;
using streamer.db.Database.DataModel;
using streamer.Features.interfaces.services;
using timeOrg.Features.User.ExchangeRefreshToken;

namespace streamer.Features.User.ExchangeRefreshToken
{
    public sealed class ExchangeRefreshTokenCommandHandler : IRequestHandler<ExchangeRefreshTokenCommand, ExchangeRefreshTokenCommandResult>
    {
        private readonly IJwtTokenValidator _jwtTokenValidator;
        private readonly UserManager<StreamerDm> _userManager;
        private readonly StreamerDbContext _appDbContext;
        private readonly IJwtFactory _jwtFactory;
        private readonly ITokenFactory _tokenFactory;


        public ExchangeRefreshTokenCommandHandler(IJwtTokenValidator jwtTokenValidator, UserManager<StreamerDm> userManager, StreamerDbContext appDbContext, IJwtFactory jwtFactory, ITokenFactory tokenFactory)
        {
            _jwtTokenValidator = jwtTokenValidator;
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
            _jwtFactory = jwtFactory;
            _tokenFactory = tokenFactory;
        }

        public async Task<ExchangeRefreshTokenCommandResult> Handle(ExchangeRefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var cp = _jwtTokenValidator.GetPrincipalFromToken(request.AccessToken, request.SigningKey);

            // invalid token/signing key was passed and we can't extract user claims
            if (cp != null)
            {
                var id = cp.Claims.First(c => c.Type == "id");
                var user = await _userManager.FindByIdAsync(id.Value);

                if (hasValidRefreshToken(request.RefreshToken))
                {
                    var identity = _jwtFactory.GenerateClaimsIdentity(user.UserName, user.Id);
                    var auth_token = await _jwtFactory.GenerateEncodedToken(user.UserName, identity);
                    var refreshToken = _tokenFactory.GenerateToken();
                    removeRefreshToken(request.RefreshToken, user.Id); // delete the token we've exchanged
                    addRefreshToken(refreshToken, user.Id, ""); // add the new one

                    return new ExchangeRefreshTokenCommandResult(auth_token,request. RefreshToken);
                    
                }
            }
            return new ExchangeRefreshTokenCommandResult(new[] { new Error("login_failure", "Invalid token.") });
        }

        private bool hasValidRefreshToken(string refreshToken)
        {
            return _appDbContext.RefreshTokens.Where(rt => rt.Token == refreshToken && rt.Active).Count() > 1;
        }

        private void addRefreshToken(string refreshToken, Guid userId, string remoteIpAddress, double daysToExpire = 5)
        {
            var rt = new RefreshTokenDm(refreshToken, DateTime.UtcNow.AddDays(daysToExpire), userId, remoteIpAddress);
            _appDbContext.RefreshTokens.Add(rt);
            _appDbContext.SaveChanges();
        }

        public void removeRefreshToken(string refreshToken, Guid userId)
        {
            _appDbContext.RefreshTokens.Remove(_appDbContext.RefreshTokens.Where(t => t.Token == refreshToken && t.UserId == userId).FirstOrDefault());
            _appDbContext.SaveChanges();
        }
    }
}
